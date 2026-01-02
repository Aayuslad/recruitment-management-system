
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class CreateCandidateWithExcelDocHandler : IRequestHandler<CreateCandidatesWithExcelDocCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserContext _userContext;

        public CreateCandidateWithExcelDocHandler(ICandidateRepository candidateRepository, IUserContext userContext)
        {
            _candidateRepository = candidateRepository;
            _userContext = userContext;
        }

        public Task<Result> Handle(CreateCandidatesWithExcelDocCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}