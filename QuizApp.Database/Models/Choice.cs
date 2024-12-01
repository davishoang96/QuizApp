namespace QuizApp.Database.Models;

public class Choice : BaseModel
{
    public string ChoiceTitle { get; set; } = string.Empty;
    
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}