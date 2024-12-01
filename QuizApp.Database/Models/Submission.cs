namespace QuizApp.Database.Models;

public class Submission : BaseModel
{
    // Navigation property for the related quiz
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;

    // Navigation property for user answers
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    // Automatically calculated score
    public int Score { get; set; }

    // To get user detail
    public string UserId { get; set; } = null!;

    // Timestamp for when the quiz was submitted
    public DateTime SubmittedAt { get; set; }
}