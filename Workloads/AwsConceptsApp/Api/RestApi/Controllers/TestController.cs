using Application.Common.Test;

namespace RestApi.Controllers;

[Route("api/[controller]")]
public class TestController : ApiControllerBase
{

    [HttpGet]
    public async Task<string> Get()
    {
        return await Mediator.Send(new GetSuccessQuery("success"));
    }


    [HttpGet("raiseerror")]
    public async Task<string> RaiseError()
    {
        return await Mediator.Send(new RaiseErrorCommand("error command"));
    }

    // POST api/values
    [HttpPost]
    public string Post([FromBody] string value)
    {
        return $"you posted {value}";
    }

}