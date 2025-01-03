namespace QuizApp.Database.Models;

public class Question : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();
}