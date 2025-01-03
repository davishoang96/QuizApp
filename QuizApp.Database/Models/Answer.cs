namespace QuizApp.Database.Models;

public class Answer : BaseModel
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}