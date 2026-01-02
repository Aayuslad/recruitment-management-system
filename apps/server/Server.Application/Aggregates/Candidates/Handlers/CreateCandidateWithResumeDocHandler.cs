
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class CreateCandidateWithResumeDocHandler : IRequestHandler<CreateCandidateWithResumeDocCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserContext _userContext;

        public CreateCandidateWithResumeDocHandler(ICandidateRepository candidateRepository, IUserContext userContext)
        {
            _candidateRepository = candidateRepository;
            _userContext = userContext;
        }

        public Task<Result> Handle(CreateCandidateWithResumeDocCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}