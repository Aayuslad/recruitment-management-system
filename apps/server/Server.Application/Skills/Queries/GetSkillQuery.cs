using MediatR;

using Server.Application.Skills.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Skills.Queries
{
    public class GetSkillQuery : IRequest<Result<SkillDetailDTO>>
    {
        public GetSkillQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}