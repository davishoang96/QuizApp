using FastEndpoints;

namespace BlazorQuizApp.Endpoints;

public class TestEndpoint : Endpoint<EmptyRequest, string>
{
    public override void Configure()
    {
        Get("/api/test");
        AllowAnonymous();
    }

    public override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        Response = "data accessed!";
        return Task.CompletedTask;
    }
}
