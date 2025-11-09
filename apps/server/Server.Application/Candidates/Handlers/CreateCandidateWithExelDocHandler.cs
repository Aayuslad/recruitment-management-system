
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Candidates.Commands;
using Server.Core.Results;

namespace Server.Application.Candidates.Handlers
{
    internal class CreateCandidateWithExelDocHandler : IRequestHandler<CreateCandidatesWithExelDocCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCandidateWithExelDocHandler(ICandidateRepository candidateRepository, IHttpContextAccessor contextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = contextAccessor;
        }

        public Task<Result> Handle(CreateCandidatesWithExelDocCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}