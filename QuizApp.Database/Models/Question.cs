namespace QuizApp.Database.Models;

public class Question : BaseModel
{
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public ICollection<Answer> Answers { get; set; }
}