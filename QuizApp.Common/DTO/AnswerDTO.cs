namespace QuizApp.Common.DTO;

public class AnswerDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
