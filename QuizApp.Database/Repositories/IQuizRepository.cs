using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IQuizRepository
{
    Task<int> SaveOrUpdateQuiz(QuizDTO quizDTO);
    Task<int> CreateQuestion(int quizId, string question, List<(string text, bool isCorrect)> answers);
    Task<IEnumerable<QuestionDTO>> GetQuestions();
}