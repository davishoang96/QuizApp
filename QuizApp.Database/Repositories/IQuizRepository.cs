using QuizApp.Common.DTO;

namespace QuizApp.Database.Repositories;

public interface IQuizRepository
{
    Task<int> SaveOrUpdateQuiz(QuizDTO quizDTO);
    Task<int> SaveOrUpdateQuestion(QuestionDTO questionDto);
    Task<IEnumerable<QuestionDTO>> GetQuestions();
    Task<IEnumerable<QuizDTO>> GetAllQuizzes();
}