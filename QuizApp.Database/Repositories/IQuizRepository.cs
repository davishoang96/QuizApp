using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IQuizRepository
{
    Task<int> CreateQuiz(string name);
    Task<int> CreateQuestion(int quizId, string question, List<(string text, bool isCorrect)> answers);
    Task<IEnumerable<QuestionDTO>> GetQuestions();
    Task<int> SaveSubmissionAsync(int userId, int quizId, IEnumerable<UserAnswerDTO> userAnswers);
}