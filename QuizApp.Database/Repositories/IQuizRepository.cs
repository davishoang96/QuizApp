using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IQuizRepository
{
    Task<bool> DeleteQuiz(int quizId);
    Task<int> SaveOrUpdateQuiz(QuizDTO quizDTO);
    Task<int> SaveOrUpdateQuestion(QuestionDTO questionDto);
    Task<int> SaveOrUpdateQuestion(IEnumerable<QuestionDTO> questionDTOs, int quizId);
    Task<IEnumerable<QuestionDTO>> GetQuestions();
    Task<IEnumerable<QuizDTO>> GetAllQuizzes();
    Task<IEnumerable<QuestionDTO>> GetQuestionsByQuizId(int quizId);
    Task<int> SaveSubmissionAsync(int userId, int quizId, IEnumerable<UserAnswerDTO> userAnswers);
}