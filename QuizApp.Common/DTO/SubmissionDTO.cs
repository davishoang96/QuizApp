namespace QuizApp.Common.DTO;

public class SubmissionDTO
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public int QuizId { get; set; }
    public DateTime AttemptDate { get; set; }
    public double Score { get; set; }
}
