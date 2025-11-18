using MediatR;

using Server.Core.Results;

namespace Server.Application.Interviews.Commands
{
    public class DeleteInterviewCommand : IRequest<Result>
    {
        public DeleteInterviewCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}