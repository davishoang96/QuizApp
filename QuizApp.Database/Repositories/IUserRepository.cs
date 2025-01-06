using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IUserRepository
{
    Task<int> CreateUser(UserDTO createUserDTO);
    Task<bool> IsUserExist(string userId);
} 