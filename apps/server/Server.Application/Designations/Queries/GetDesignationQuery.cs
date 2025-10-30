using MediatR;

using Server.Application.Designations.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Designations.Queries
{
    public class GetDesignationQuery : IRequest<Result<DesignationDetailDTO>>
    {
        public GetDesignationQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}