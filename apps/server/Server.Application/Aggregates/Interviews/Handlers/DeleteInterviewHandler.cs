
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class DeleteInterviewHandler : IRequestHandler<DeleteInterviewCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUserContext _userContext;

        public DeleteInterviewHandler(IInterviewRepository interviewRepository, IUserContext userContext)
        {
            _interviewRepository = interviewRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteInterviewCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interview = await _interviewRepository.GetByIdAsync(request.Id, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundException("Interview Not Found");
            }

            // step 2: delete
            await _interviewRepository.DeleteAsync(interview, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}