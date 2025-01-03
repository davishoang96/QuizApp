namespace QuizApp.Database.Models;

public class Submission : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public DateTime AttemptDate { get; set; }
    public int Score { get; set; }
}