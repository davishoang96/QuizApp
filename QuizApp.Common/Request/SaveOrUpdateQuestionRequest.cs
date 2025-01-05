using QuizApp.Common.DTO;

namespace QuizApp.Common.Request;

public class SaveOrUpdateQuestionRequest
{
    public int QuizId { get; set; }
    public int QuestionId { get; set; }
    public IEnumerable<QuestionDTO> QuestionDTOs { get; set; }
}
