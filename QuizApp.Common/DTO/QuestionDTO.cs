namespace QuizApp.Common.DTO;

public class QuestionDTO
{
    public int QuizId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public IEnumerable<AnswerDTO> Answers { get; set; } = new List<AnswerDTO>();
}
