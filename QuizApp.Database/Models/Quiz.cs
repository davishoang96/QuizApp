namespace QuizApp.Database.Models;

public class Quiz : BaseModel
{
    public string QuizName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}