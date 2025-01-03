using QuizApp.Database;
using QuizApp.Database.Repositories;
using Xunit;
using FluentAssertions;
using QuizApp.Database.Models;

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
        var model = context.Quizzes.FirstOrDefault();
        model?.QuizName.Should().Be(name);
        context.Quizzes.ToList().Count.Should().Be(1);
    }

    [Fact]
    public async Task CreateQuestionOK()
    {
        // Arrange
        var quizId = 1;
        var question = "Who create macos?";
        using (var setupContext = new QuizContext(Options))
        {
            // Ensure the database is clean
            await setupContext.Database.EnsureDeletedAsync();
            await setupContext.Database.EnsureCreatedAsync();

            // Seed the required quiz
            setupContext.Quizzes.Add(new Quiz
            {
                Id = quizId,
                QuizName = "Technology Quiz",
                Description = "A quiz about technology.",
            });
            await setupContext.SaveChangesAsync();
        }

        // Act
        var result = await quizRepository.CreateQuestion(quizId, question);
        
        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        var model = context.Questions.SingleOrDefault(s => s.Id == 4);
        model?.Title.Should().Be(question);
    }
}