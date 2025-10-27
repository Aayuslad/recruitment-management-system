using MediatR;

using Server.Application.DTOs;
using Server.Core.Results;

namespace Server.Application.Skills.Queries
{
    public class GetSkillQuery : IRequest<Result<SkillDTO>>
    {
        public GetSkillQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
