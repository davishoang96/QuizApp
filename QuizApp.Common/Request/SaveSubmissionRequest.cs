using QuizApp.Common.DTO;

namespace QuizApp.Common.Request;

public class SaveSubmissionRequest
{
    public int QuizId { get; set; }
    public int UserId { get; set; }
    public IEnumerable<UserAnswerDTO> UserAnswerDTOs { get; set; }
}
