namespace Application.Common.Test
{
    public class RaiseErrorCommand : IRequest<string>
    {
        public RaiseErrorCommand(string ErrorMessage)
        {
            this.ErrorMessage = ErrorMessage;
        }
        public string ErrorMessage { get; }
    }
    public class RaiseErrorCommandHandler : IRequestHandler<RaiseErrorCommand, string>
    {
        public RaiseErrorCommandHandler()
        {
        }
        Task<string> IRequestHandler<RaiseErrorCommand, string>.Handle(RaiseErrorCommand request, CancellationToken cancellationToken)
        {
            throw new Exception(request.ErrorMessage);
        }
    }
}
