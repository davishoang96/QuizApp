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
    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(q => q.Id);
            entity.HasMany(q => q.Questions)
                  .WithOne(q => q.Quiz)
                  .HasForeignKey(q => q.QuizId) // Explicit foreign key configuration (optional)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Submission>()
            .HasKey(uq => uq.Id);

        modelBuilder.Entity<Submission>()
            .HasOne(uq => uq.User)
            .WithMany(u => u.Submissions)
            .HasForeignKey(uq => uq.UserId);

        modelBuilder.Entity<Submission>()
            .HasOne(uq => uq.Quiz)
            .WithMany(q => q.Submissions)
            .HasForeignKey(uq => uq.QuizId);
    }
}