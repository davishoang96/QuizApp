using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using QuizApp.Common.DTO;

namespace QuizApp.Services;

public interface IUserService
{
    Task<UserDTO> CreateUserDTO();    
}

public class UserService : IUserService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public UserService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<UserDTO> CreateUserDTO()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if(authState.User.Identity != null)
        {
            var user = authState.User;
            return new UserDTO
            {
                FullName = user.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Username = user.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value,
                UserId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                Role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                Email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
            };
        }

        return null;
    }
}
