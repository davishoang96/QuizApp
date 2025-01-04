using FluentAssertions;
using QuizApp.Common.DTO;
using QuizApp.Database;
using QuizApp.Database.Models;
using QuizApp.Database.Repositories;
using Xunit;
using TheoryAttribute = Xunit.TheoryAttribute;

namespace QuizApp.Test;

public sealed class QuizRepositoryTest : BaseRepoTest
{
    private readonly QuizRepository quizRepository;
    public QuizRepositoryTest()
    {
        quizRepository = new QuizRepository(new QuizContext(Options));
    }

    [Fact]
    public async Task GetAllQuizzesOK()
    {
        // Arrange
        using (var setupContext = new QuizContext(Options))
        {
            // Ensure the database is clean
            await setupContext.Database.EnsureDeletedAsync();
            await setupContext.Database.EnsureCreatedAsync();

            // Seed the required quiz
            setupContext.Quizzes.Add(new Quiz
            {
                QuizName = "General Knowledge",
                Description = "GK",
            });
            setupContext.Quizzes.Add(new Quiz
            {
                QuizName = "Science",
                Description = "Science",
            });
            setupContext.Quizzes.Add(new Quiz
            {
                QuizName = "PTE",
                Description = "PTE",
            });
            await setupContext.SaveChangesAsync();
        }

        // Act
        var result = await quizRepository.GetAllQuizzes();

        // Assert
        result.Count().Should().Be(3);
    }

    [Fact]
    public async Task CreateQuizOK()
    {
        // Arrange
        var name = "PTE";
        
        // Act
        var result = await quizRepository.SaveOrUpdateQuiz(new QuizDTO
        {
            Name = name,
            Description = name,
        });
        
        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        var model = context.Quizzes.FirstOrDefault();
        model?.QuizName.Should().Be(name);
        context.Quizzes.ToList().Count.Should().Be(1);
    }

    [Fact]
    public async Task UpdateQuizOK()
    {
        // Arrange
        using (var setupContext = new QuizContext(Options))
        {
            // Ensure the database is clean
            await setupContext.Database.EnsureDeletedAsync();
            await setupContext.Database.EnsureCreatedAsync();

            // Seed the required quiz
            setupContext.Quizzes.Add(new Quiz
            {
                Id = 1,
                QuizName = "General Knowledge",
                Description = "GK",
            });
            await setupContext.SaveChangesAsync();
        }

        // Act
        var result = await quizRepository.SaveOrUpdateQuiz(new QuizDTO
        {
            Id = 1,
            Name = "Science",
            Description = "Science"
        });

        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        var model = context.Quizzes.FirstOrDefault();
        model?.QuizName.Should().Be("Science");
        model?.Description.Should().Be("Science");
    }

    [Fact]
    public async Task CreateQuestionWithAnswer()
    {
        // Arrange
        var quizId = 1;
        var questionTitle = "What is the capital of France?";
        using (var setupContext = new QuizContext(Options))
        {
            // Ensure the database is clean
            await setupContext.Database.EnsureDeletedAsync();
            await setupContext.Database.EnsureCreatedAsync();

            // Seed the required quiz
            setupContext.Quizzes.Add(new Quiz
            {
                Id = quizId,
                QuizName = "General Knowledge",
                Description = "GK",
            });
            await setupContext.SaveChangesAsync();
        }

        var answers = new List<(string Text, bool IsCorrect)>
        {
            ("Paris", true),
            ("London", false),
            ("Berlin", false),
            ("Madrid", false)
        };

        // Act
        var result = await quizRepository.CreateQuestion(quizId, questionTitle, answers);

        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        var model = context.Questions.SingleOrDefault(s => s.Id == 4);
        model?.Title.Should().Be(questionTitle);
        context.Answers.Count().Should().Be(4);
    }

    [Fact]
    public async Task GetQuestionsOK()
    {
        // Arrange
        var questionTitle = "What is the capital of France?";

        using (var setupContext = new QuizContext(Options))
        {
            // Ensure the database is clean
            await setupContext.Database.EnsureDeletedAsync();
            await setupContext.Database.EnsureCreatedAsync();

            // Seed the required quiz
            setupContext.Quizzes.Add(new Quiz
            {
                Id = 1,
                QuizName = "General Knowledge",
                Description = "GK",
            });

            setupContext.Questions.Add(new Question
            {
                QuizId = 1,
                Title = questionTitle,
                Answers = new List<Answer>
                {
                    new Answer { Text = "Paris", IsCorrect = true },
                    new Answer { Text = "London", IsCorrect = false }
                }
            });

            await setupContext.SaveChangesAsync();
        }

        // Act
        var result = await quizRepository.GetQuestions();

        // Assert
        result.Should().NotBeEmpty();
        result.First().Title.Should().Be(questionTitle);    
        result.First().Answers.Count().Should().Be(2);
        result.First().Answers.First(s => s.IsCorrect).Text.Should().Be("Paris");
    }

    [Theory]
    [InlineData(100, 12)]
    [InlineData(67, 11)]
    public async Task SaveSubmissionOK(int score, int selectedAnswerId)
    {
        // Arrange
        using (var setupContext = new QuizContext(Options))
        {
            // Ensure the database is clean
            await setupContext.Database.EnsureDeletedAsync();
            await setupContext.Database.EnsureCreatedAsync();

            setupContext.Users.Add(new User
            {
                Id = 1,
                Email = "johnDoe@gmail.com",
                JoinDate = DateTime.Now,
                Role = "Student",
                Username = "johndoe",
                FullName = "John Doe",
            });

            // Seed the required quiz
            setupContext.Quizzes.Add(new Quiz
            {
                Id = 1,
                QuizName = "General Knowledge",
                Description = "GK",
            });

            setupContext.Questions.Add(new Question
            {
                QuizId = 1,
                Title = "What is the capital of France?",
                Answers = new List<Answer>
                {
                    new Answer { Text = "Paris", IsCorrect = true },
                    new Answer { Text = "London", IsCorrect = false },
                    new Answer { Text = "Ha Noi", IsCorrect = false },
                    new Answer { Text = "Essex", IsCorrect = false },
                }
            });

            setupContext.Questions.Add(new Question
            {
                QuizId = 1,
                Title = "What is the capital of Australia?",
                Answers = new List<Answer>
                {
                    new Answer { Text = "Perth", IsCorrect = false },
                    new Answer { Text = "Darwin", IsCorrect = false },
                    new Answer { Text = "Sydney", IsCorrect = false },
                    new Answer { Text = "Canberra", IsCorrect = true }
                }
            });

            setupContext.Questions.Add(new Question
            {
                QuizId = 1,
                Title = "How many planets in our solar system?",
                Answers = new List<Answer>
                {
                    new Answer { Text = "1", IsCorrect = false },
                    new Answer { Text = "5", IsCorrect = false },
                    new Answer { Text = "9", IsCorrect = false },
                    new Answer { Text = "8", IsCorrect = true }
                }
            });

            await setupContext.SaveChangesAsync();
        }

        var listOfAnswers = new List<UserAnswerDTO>
        {
            new UserAnswerDTO
            {
                QuestionId = 1,
                SelectedAnswerId = 1,
            },
            new UserAnswerDTO
            {
                QuestionId = 2,
                SelectedAnswerId = 8,
            },
            new UserAnswerDTO
            {
                QuestionId = 3,
                SelectedAnswerId = selectedAnswerId,
            }
        };

        // Act
        var result = await quizRepository.SaveSubmissionAsync(1, 1, listOfAnswers);

        // Assert
        result.Should().NotBe(0);
        using var context = new QuizContext(Options);
        var submission = context.Submissions.First();
        context.Submissions.Count().Should().Be(1);
        submission.UserId.Should().Be(1);
        submission.Score.Should().Be(score);
    }
}