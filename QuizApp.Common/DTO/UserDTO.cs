namespace QuizApp.Common.DTO;

public class UserDTO
{
    public string FullName { get; set; }
    public string Username { get; set; }

    // Auth0 user id
    public string UserId { get; set; }

    public string Role {  get; set; }
    public string Email { get; set; }
}
