using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Common.DTO;
using QuizApp.Database.Repositories;

namespace BlazorQuizApp.Endpoints.User;

[HttpPost("user/create"), Authorize]
public class CreateUserEndpoint : Endpoint<UserDTO, int>
{
    private readonly IUserRepository userRepository;
    public CreateUserEndpoint(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public override async Task HandleAsync(UserDTO dto, CancellationToken ct)
    {
        var result = await userRepository.CreateUser(dto);
        if (result < 0)
        {
            ThrowError("Cannot create user");
        }

        await SendAsync(result);
    }
}
