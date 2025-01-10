using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IUserRepository
{
    Task<int> CreateUser(UserDTO createUserDTO);
    Task<int> IsUserExist(string userId);
}