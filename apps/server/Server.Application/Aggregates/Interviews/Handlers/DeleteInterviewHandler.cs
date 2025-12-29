
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class DeleteInterviewHandler : IRequestHandler<DeleteInterviewCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteInterviewHandler(IInterviewRepository interviewRepository, IHttpContextAccessor contextAccessor)
        {
            _interviewRepository = interviewRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteInterviewCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

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