using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IQuizRepository
{
    Task<bool> DeleteQuiz(int quizId);
    Task<int> SaveOrUpdateQuiz(QuizDTO quizDTO);
    Task<int> SaveOrUpdateQuestion(QuestionDTO questionDto);
    Task<IEnumerable<QuestionDTO>> GetQuestions();
    Task<IEnumerable<QuizDTO>> GetAllQuizzes();
    Task<int> SaveSubmissionAsync(int userId, int quizId, IEnumerable<UserAnswerDTO> userAnswers);
}