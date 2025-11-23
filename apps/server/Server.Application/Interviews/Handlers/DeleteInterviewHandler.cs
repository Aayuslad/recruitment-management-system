
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.Interviews.Commands;
using Server.Core.Results;

namespace Server.Application.Interviews.Handlers
{
    internal class DeleteInterviewHandler : IRequestHandler<DeleteInterviewCommand, Result>
    {
        private readonly IInterviewRespository _interviewRespository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteInterviewHandler(IInterviewRespository interviewRespository, IHttpContextAccessor contextAccessor)
        {
            _interviewRespository = interviewRespository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteInterviewCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch the interviw
            var interview = await _interviewRespository.GetByIdAsync(request.Id, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundExeption("Interview Not Found");
            }

            // step 2: delete
            await _interviewRespository.DeleteAsync(interview, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}