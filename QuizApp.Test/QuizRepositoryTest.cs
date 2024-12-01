using QuizApp.Database;
using QuizApp.Database.Repositories;
using Xunit;
using FluentAssertions;

namespace QuizApp.Test;

public sealed class QuizRepositoryTest : BaseRepoTest
{
    private readonly QuizRepository quizRepository;
    public QuizRepositoryTest()
    {
        quizRepository = new QuizRepository(new QuizContext(Options));
    }

    [Fact]
    public async Task CreateQuizOK()
    {
        // Arrange
        var name = "PTE";
        
        // Act
        var result = await quizRepository.CreateQuiz(name);
        
        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        var model = context.Quizzes.SingleOrDefault(s => s.Id == 3);
        model?.QuizName.Should().Be(name);
        context.Quizzes.ToList().Count.Should().Be(3);
    }

    [Fact]
    public async Task CreateQuestionOK()
    {
        // Arrange
        var quizId = 1;
        var question = "Who create macos?";
        
        // Act
        var result = await quizRepository.CreateQuestion(quizId, question);
        
        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        var model = context.Questions.SingleOrDefault(s => s.Id == 4);
        model?.Title.Should().Be(question);
    }
}