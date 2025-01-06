using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace BlazorQuizApp.Endpoints;

[HttpPost("api/secure-endpoint"), Authorize]
public class GetScoreEndpoint : Endpoint<EmptyRequest, string>
{
    public override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        Response = "Secure data accessed!";
        return Task.CompletedTask;
    }
}
