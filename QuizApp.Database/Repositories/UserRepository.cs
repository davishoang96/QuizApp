using Microsoft.EntityFrameworkCore;
using QuizApp.Common.DTO;
using QuizApp.Database.Models;

namespace QuizApp.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly QuizContext db;
    
    public UserRepository(QuizContext db)
    {
        this.db = db;
    }

    public async Task<int> CreateUser(UserDTO createUserDTO)
    {
        var user = await db.AddAsync(new User
        {
            UserId = createUserDTO.UserId,
            FullName = createUserDTO.FullName,
            Username = createUserDTO.Username,
            Role = createUserDTO.Role,
            Email = createUserDTO.Email,
            JoinDate = DateTime.Now,
        });

        await db.SaveChangesAsync();

        return user.Entity.Id;
    }

    public async Task<bool> IsUserExist(string userId)
    {
        return await db.Users.AnyAsync(u => u.UserId == userId);
    }
}