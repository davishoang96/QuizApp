namespace QuizApp.Database.Models;

public class User : BaseModel
{
    // Auth0 user Id
    public string UserId { get; set; }

    public string Username { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; } 
    public string Email { get; set; }
    public DateTime JoinDate { get; set; }
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}   