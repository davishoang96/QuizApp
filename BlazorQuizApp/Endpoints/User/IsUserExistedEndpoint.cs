using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Common.Request;
using QuizApp.Database.Repositories;

namespace BlazorQuizApp.Endpoints.User;

[HttpGet("user/isuserexisted"), Authorize]
public class IsUserExistedEndpoint : Endpoint<CheckUserExistRequest, bool>
{
    private readonly IUserRepository userRepository;
    public IsUserExistedEndpoint(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public override async Task HandleAsync(CheckUserExistRequest r, CancellationToken ct)
    {
        await SendAsync(await userRepository.IsUserExist(r.UserId));
    }
}
