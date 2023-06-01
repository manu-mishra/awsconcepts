global using MediatR;
global using Microsoft.AspNetCore.Mvc;
using OpenSearch.Client;

namespace RestApi.Controllers
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        private IMediator? _mediator;

        protected IMediator Mediator => _mediator ??= InitializeMediator();
        IMediator InitializeMediator()
        {
            IMediator? mediator = HttpContext?.RequestServices?.GetService<IMediator>();
            if (mediator != null)
                return mediator;
            throw new Exception("Unrechable : Could not initialize mediator");
        }
        protected void SetContinuationTokens(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;
            Response.Headers.Add("X-Continuation-Token", token);
        }

    }
}
