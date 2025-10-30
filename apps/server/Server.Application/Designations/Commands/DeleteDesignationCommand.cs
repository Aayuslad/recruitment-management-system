using MediatR;

using Server.Core.Results;

namespace Server.Application.Designations.Commands
{
    public class DeleteDesignationCommand : IRequest<Result>
    {
        public DeleteDesignationCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}