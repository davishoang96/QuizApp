namespace QuizApp.Common.DTO;

public class QuestionDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public IEnumerable<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
}
