namespace QuizApp.Database.Models;

public class Answer : BaseModel
{
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public int ChoiceId { get; set; }
    public Choice Choice { get; set; } = null!;

    public int SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;
}