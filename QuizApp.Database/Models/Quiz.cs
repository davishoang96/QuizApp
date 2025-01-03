namespace QuizApp.Database.Models;

public class Quiz : BaseModel
{
    public string QuizName { get; set; }
    public string Description { get; set; }
    public ICollection<Question> Questions { get; set; }
}