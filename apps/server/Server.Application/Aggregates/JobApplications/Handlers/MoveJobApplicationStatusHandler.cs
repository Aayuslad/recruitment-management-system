using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities.Employees;
using Server.Domain.Entities.Interviews;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class MoveJobApplicationStatusHandler : IRequestHandler<MoveJobApplicationStatusCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IInterviewRespository _interviewRespository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;

        public MoveJobApplicationStatusHandler(
            IJobApplicationRepository jobApplicationRepository,
            IHttpContextAccessor contextAccessor,
            IInterviewRespository interviewRespository,
            IEmployeeRepository employeeRepository,
            IPositionRepository positionRepository
        )
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
            _interviewRespository = interviewRespository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public async Task<Result> Handle(MoveJobApplicationStatusCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (application is null)
            {
                throw new NotFoundExeption("Job Application Not Found.");
            }


            // step 2: move to given stage

            // for status move actions which includes (move to shortlisted, offered, interviewd, rejected, hired, on hold)
            if (!string.IsNullOrWhiteSpace(request.MoveTo.ToString()) && string.IsNullOrWhiteSpace(request.Action.ToString()))
            {
                // TODO: make domain event for creating interviews
                // Creating interviews using templates when application is moved to shortlisted
                if (request.MoveTo == JobApplicationStatus.Shortlisted)
                {
                    // create interviews
                    var interviews = new List<Interview>();

                    foreach (var interviewTemplate in application.JobOpening.InterviewRounds)
                    {
                        var newInterviewId = Guid.NewGuid();

                        interviews.Add(Interview.Create(
                            id: newInterviewId,
                            jobApplicationId: application.Id,
                            roundNumber: interviewTemplate.RoundNumber,
                            interviewType: interviewTemplate.Type,
                            scheduledAt: null,
                            durationInMinutes: interviewTemplate.DurationInMinutes,
                            meetingLink: null,
                            status: InterviewStatus.NotScheduled,
                            participants: interviewTemplate.PanelRequirements.Select(
                                selector: x => InterviewParticipant.Create(
                                        id: null,
                                        interviewId: newInterviewId,
                                        userId: application.JobOpening.JobOpeningInterviewers.Where(y => y.Role == x.Role).ElementAt(
                                                new System.Random().Next() % application.JobOpening.JobOpeningInterviewers.Count(y => y.Role == x.Role)
                                            ).UserId,
                                        role: x.Role
                                    )
                            ).ToList()
                        ));
                    }

                    await _interviewRespository.AddRangeAsync(interviews, cancellationToken);
                }

                // TODO: make domain event for adding candidate in position
                // Adding the link of candidate-position when an application is offered
                if (request.MoveTo == JobApplicationStatus.Offered)
                {
                    var positions = await _positionRepository.GetAllByBatchIdAsync(application.JobOpening.PositionBatchId, cancellationToken);

                    var vacantPosition = positions.FirstOrDefault(x => x.Status == PositionStatus.Open);

                    if (vacantPosition != null)
                    {
                        vacantPosition.CloseWithCandidate(application.CandidateId, Guid.Parse(userIdString));
                        await _positionRepository.UpdateAsync(vacantPosition, cancellationToken);
                    }
                }

                // TODO: make domain event for removing candidate from position
                // Removing the link of candidate-position when an offerd application is rejected (the closed position is reopened)
                if (request.MoveTo == JobApplicationStatus.Rejected)
                {
                    var positions = await _positionRepository.GetAllByBatchIdAsync(application.JobOpening.PositionBatchId, cancellationToken);

                    var occupiedPosition = positions.FirstOrDefault(x => x.ClosedByCandidate == application.CandidateId);

                    if (occupiedPosition != null)
                    {
                        occupiedPosition.ReOpen(Guid.Parse(userIdString), null);
                        await _positionRepository.UpdateAsync(occupiedPosition, cancellationToken);
                    }
                }

                // TODO: make domain event for creating employee
                // Moving candidate to employee record when application is moved to hired
                if (request.MoveTo == JobApplicationStatus.Hired)
                {
                    var employee = Employee.Create(
                        id: null,
                        designationId: application.JobOpening.PositionBatch.DesignationId,
                        email: application.Candidate.Email,
                        firstName: application.Candidate.FirstName,
                        middleName: application.Candidate.MiddleName,
                        lastName: application.Candidate.LastName,
                        contactNumber: application.Candidate.ContactNumber,
                        dob: application.Candidate.Dob
                    );

                    await _employeeRepository.AddAsync(employee, cancellationToken);
                }

                application.MoveStatus(Guid.Parse(userIdString), (JobApplicationStatus)request.MoveTo);
            }

            // for status move actions which includes (un-hold, rollback)
            if (string.IsNullOrWhiteSpace(request.MoveTo.ToString()) && !string.IsNullOrWhiteSpace(request.Action.ToString()))
            {
                // TODO: resolve-issue: rollback can only be done at one prev stage, if tried multiple it will stuck in loop. to prevent this, option of rollback is only given at the rejected state for now.
                if (request.Action == JobApplicationStatusActions.UnHold || request.Action == JobApplicationStatusActions.RollBack)
                {
                    var prevState = application.StatusMoveHistories.OrderBy(x => x.MovedAt).SkipLast(1).Last().StatusMovedTo;

                    // if application was on offered stage before rejection, the position should be re-closed when rolling back (as while rejection the position is reopened)
                    if (request.Action == JobApplicationStatusActions.RollBack && prevState == JobApplicationStatus.Offered)
                    {
                        var positions = await _positionRepository.GetAllByBatchIdAsync(application.JobOpening.PositionBatchId, cancellationToken);

                        var vacantPosition = positions.FirstOrDefault(x => x.Status == PositionStatus.Open);

                        if (vacantPosition != null)
                        {
                            vacantPosition.CloseWithCandidate(application.CandidateId, Guid.Parse(userIdString));
                            await _positionRepository.UpdateAsync(vacantPosition, cancellationToken);
                        }
                    }

                    application.MoveStatus(Guid.Parse(userIdString), prevState);
                }
            }

            // step 3: persist entity
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}