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
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // BaseModel Configuration
        modelBuilder.Entity<BaseModel>()
            .HasKey(b => b.Id);

        // User Configuration
        modelBuilder.Entity<User>()
            .HasMany(u => u.Submissions)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Quiz Configuration
        modelBuilder.Entity<Quiz>()
            .HasMany(q => q.Questions)
            .WithOne(q => q.Quiz)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question Configuration
        modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
            .WithOne(c => c.Question)
            .HasForeignKey(c => c.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Choice Configuration
        modelBuilder.Entity<Choice>()
            .Property(c => c.ChoiceTitle)
            .IsRequired()
            .HasMaxLength(500); // Example length limit, adjust as needed

        // Submission Configuration
        modelBuilder.Entity<Submission>()
            .HasMany(s => s.Answers)
            .WithOne(a => a.Submission)
            .HasForeignKey(a => a.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Answer Configuration
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Question if Answer exists

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Choice)
            .WithMany()
            .HasForeignKey(a => a.ChoiceId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Choice if Answer exists

        //modelBuilder.Entity<Quiz>().HasData(
        //    new Quiz
        //    {
        //        Id = 1,
        //        QuizName = "General Knowledge",
        //        Description = "GK"
        //    },
        //    new Quiz
        //    {
        //        Id = 2,
        //        QuizName = "Science Trivia",
        //        Description = "ST",
        //    }
        //);
        
        //modelBuilder.Entity<Question>().HasData(
        //    new Question
        //    {
        //        Id = 1,
        //        QuizId = 1,
        //        Title = "What is the capital of France?",
        //    },
        //    new Question
        //    {
        //        Id = 2,
        //        QuizId = 1,
        //        Title = "Who wrote 'Romeo and Juliet'?",
        //    },
        //    new Question
        //    {
        //        Id = 3,
        //        QuizId = 2,
        //        Title = "What is the chemical symbol for water?",
        //    }
        //);
    }
}