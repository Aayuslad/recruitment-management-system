
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class MoveInterviewStatusHandler : IRequestHandler<MoveInterviewStatusCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public MoveInterviewStatusHandler(IInterviewRepository interviewRepository, IJobApplicationRepository jobApplicationRepository)
        {
            _interviewRepository = interviewRepository;
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<Result> Handle(MoveInterviewStatusCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interview = await _interviewRepository.GetByIdAsync(request.InterviewId, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundException("Interview Not Found.");
            }

            // step 2: move the status
            if (request.MoveTo == InterviewStatus.Completed)
            {
                // TODO: make domain event for this task
                // If all the interviews are completed (this is the last one to complete) then update the job application status to interviwed
                var JobApplicationInterviews = await _interviewRepository.GetAllByJobApplicationIdAsync(interview.JobApplicationId, cancellationToken);

                var isAllCompleted = JobApplicationInterviews.Where(x => x.Id != interview.Id).All(x => x.Status == InterviewStatus.Completed);
                if (isAllCompleted)
                {
                    var jobApplication = await _jobApplicationRepository.GetByIdAsync(interview.JobApplicationId, cancellationToken);
                    if (jobApplication is null)
                    {
                        throw new NotFoundException("Job Application Not Found.");
                    }
                    jobApplication.MoveStatusBySystem(JobApplicationStatus.Interviewed);
                    await _jobApplicationRepository.UpdateAsync(jobApplication, cancellationToken);
                }

                interview.MoveStatus(InterviewStatus.Completed);
            }

            if (request.MoveTo == InterviewStatus.Scheduled)
            {
                // TODO: make saprate route for adding meeting link
                interview.Schedule(request.ScheduledAt, request.MeetingLink);
                interview.MoveStatus(InterviewStatus.Scheduled);
            }

            // step 3: persist the entity
            await _interviewRepository.UpdateAsync(interview, cancellationToken);

            // step 4: return the result
            return Result.Success();
        }
    }
}