using System.Data.Common;
using FluentAssertions;
using QuizApp.Database;
using QuizApp.Database.Repositories;
using Xunit;

namespace QuizApp.Test;

public sealed class UserRepositoryTest : BaseRepoTest
{
    private readonly UserRepository repo;

    public UserRepositoryTest()
    {
        repo = new UserRepository(new QuizContext(Options));
    }

    //[Fact]
    //public async Task CreateUserOK()
    //{
    //    // Arrange
    //    var username = "crystark";

    //    // Act
    //    var result = await repo.CreateUser(username);
        
    //    // Assert
    //    result.Should().NotBe(0);
    //    using var context = new QuizContext(Options);
    //    context.Users.Should().HaveCount(1);
    //    var newUser = context.Users.Single(s=>s.Username == username);
    //    newUser.Id.Should().Be(1);
    //    newUser.Role.Should().Be("student");
    //}
}