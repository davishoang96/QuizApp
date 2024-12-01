using QuizApp.Database.Models;

namespace QuizApp.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly QuizContext db;
    
    public UserRepository(QuizContext db)
    {
        this.db = db;
    }
    
    public async Task<int> CreateUser(string username, string role = "student")
    {
        var user = await db.AddAsync(new User
        {
            Username = username,
            Role = role
        });

        await db.SaveChangesAsync();

        return user.Entity.Id;
    }
}