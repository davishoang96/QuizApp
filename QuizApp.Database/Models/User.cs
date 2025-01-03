namespace QuizApp.Database.Models;

public class User : BaseModel
{
    public string Username { get; set; }
    public string Role { get; set; } 
    public string Email { get; set; }
    public DateTime JoinDate { get; set; }
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}   