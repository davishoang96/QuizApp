using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IUserRepository
{
    Task<int> CreateUser(string username, string role = "student");
    Task<int> CreateUser(CreateUserDTO createUserDTO);
}