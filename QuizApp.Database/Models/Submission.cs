namespace QuizApp.Database.Models;

public class Submission : BaseModel
{
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    public int Score { get; set; }
    public DateTime SubmittedAt { get; set; }
}