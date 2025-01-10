using System.Data.Common;
using FluentAssertions;
using QuizApp.Database;
using QuizApp.Database.Models;
using QuizApp.Database.Repositories;
using Xunit;
using TheoryAttribute = Xunit.TheoryAttribute;

namespace QuizApp.Test;

public sealed class UserRepositoryTest : BaseRepoTest
{
    private readonly UserRepository repo;

    public UserRepositoryTest()
    {
        repo = new UserRepository(new QuizContext(Options));
    }

    [Theory]
    [InlineData("1wXpQEzY3G", true)]
    [InlineData("1wXpQEzY33", false)]

    public async Task IsUserExist(string userId, bool expectedResult)
    {
        using (var setupContext = new QuizContext(Options))
        {
            // Ensure the database is clean
            await setupContext.Database.EnsureDeletedAsync();
            await setupContext.Database.EnsureCreatedAsync();

            await setupContext.Users.AddAsync(new User
            {
                UserId = "1wXpQEzY3G",
                FullName = "Howard Stark",
                Email = "dav@gmail.com",
                JoinDate = DateTime.Today.AddDays(-1),
                Role = "student",
                Username = "crystark",
            });

            await setupContext.SaveChangesAsync();
        }

        // Act
        var result = await repo.IsUserExist(userId);

        // Assert
        result.Should().Be(expectedResult ? 1 : 0);
    }

    [Fact]
    public async Task CreateUserOK()
    {
        // Arrange
        var username = "crystark";
        var userId = "1wXpQEzY3G";

        // Act
        var result = await repo.CreateUser(new Common.DTO.UserDTO
        {
            UserId = userId,
            FullName = "Crystal Stark",
            Username = username,
            Role = "student",
            Email = "dav@gmail.com"
        });

        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        context.Users.Should().HaveCount(1);
        var newUser = context.Users.Single(s => s.Username == username);
        newUser.Id.Should().Be(1);
        newUser.UserId.Should().Be(userId);
        newUser.Role.Should().Be("student");
    }
}