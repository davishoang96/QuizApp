using Microsoft.EntityFrameworkCore;
using QuizApp.Database.Models;

namespace QuizApp.Database;

public class QuizContext : DbContext
{
    // Constructor that accepts DbContextOptions
    public QuizContext(DbContextOptions<QuizContext> options)
        : base(options)
    {
    }
    
    public DbSet<Quiz> Quizzes { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Choice> Choices { get; set; } = null!;
    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quiz>().HasData(
            new Quiz
            {
                Id = 1,
                QuizName = "General Knowledge"
            },
            new Quiz
            {
                Id = 2,
                QuizName = "Science Trivia"
            }
        );
        
        modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 1,
                QuizId = 1,
                Title = "What is the capital of France?",
            },
            new Question
            {
                Id = 2,
                QuizId = 1,
                Title = "Who wrote 'Romeo and Juliet'?",
            },
            new Question
            {
                Id = 3,
                QuizId = 2,
                Title = "What is the chemical symbol for water?",
            }
        );
    }
}