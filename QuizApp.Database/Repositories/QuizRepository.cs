using Microsoft.EntityFrameworkCore;
using QuizApp.Common.DTO;
using QuizApp.Database.Models;

namespace QuizApp.Database.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly QuizContext db;
    
    public QuizRepository(QuizContext db)
    {
        this.db = db;
    }

    public async Task<int> SaveOrUpdateQuiz(QuizDTO quizDTO)
    {
        Quiz quiz;

        if (!quizDTO.Id.HasValue)
        {
            quiz = new Quiz
            {
                QuizName = quizDTO.Name,
                Description = quizDTO.Description,
            };

            db.Add(quiz);
        }
        else
        {
            quiz = await db.Quizzes.FindAsync(quizDTO.Id.Value);
            if (quiz == null)
            {
                throw new InvalidOperationException($"Quiz with ID {quizDTO.Id.Value} not found.");
            }

            quiz.QuizName = quizDTO.Name;
            quiz.Description = quizDTO.Description;
            db.Entry(quiz).State = EntityState.Modified;
        }

        await db.SaveChangesAsync();

        return quiz.Id;
    }

    public async Task<int> CreateQuestion(int quizId, string question, List<(string text, bool isCorrect)> answers)
    {
        var quizModel = db.Quizzes.SingleOrDefault(x => x.Id == quizId);
        if (quizModel is null)
        {
            throw new InvalidOperationException($"Cannot find quiz with id = {quizId}");
        }

        var questionModel = new Question
        {
            QuizId = quizModel.Id,
            Quiz = quizModel,
            Title = question,
        };

        foreach (var (text, isCorrect) in answers)
        {
            var answer = new Answer
            {
                Text = text,
                IsCorrect = isCorrect,
                Question = questionModel // Automatically sets the QuestionId due to EF Core navigation properties
            };
            questionModel.Answers.Add(answer);
        }

        db.Questions.Add(questionModel);
        await db.SaveChangesAsync();

        return questionModel.Id;
    }

    public async Task<IEnumerable<QuestionDTO>> GetQuestions()
    {
        return await db.Questions
            .Include(q => q.Answers) // Include related answers
            .Select(q => new QuestionDTO
            {
                Id = q.Id,
                Title = q.Title,
                Answers = q.Answers.Select(a => new AnswerDTO
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<int> SaveSubmissionAsync(int userId, int quizId, IEnumerable<UserAnswerDTO> userAnswers)
    {
        // Validate quiz exists
        var quiz = await db.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
            .SingleOrDefaultAsync(q => q.Id == quizId);

        if (quiz == null)
            throw new InvalidOperationException($"Quiz with ID {quizId} not found.");

        // Validate all questions are answered
        if (quiz.Questions.Count != userAnswers.Count())
            throw new InvalidOperationException("User has not answered all questions.");

        // Calculate score
        var userCorrectAnswers = 0;
        foreach (var question in quiz.Questions)
        {
            var userAnswer = userAnswers.SingleOrDefault(ua => ua.QuestionId == question.Id);
            if (userAnswer == null)
                throw new InvalidOperationException($"No answer provided for question ID {question.Id}.");

            var correctAnswer = question.Answers.SingleOrDefault(a => a.IsCorrect);
            if (correctAnswer == null)
                throw new InvalidOperationException($"No correct answer defined for question ID {question.Id}.");

            if (userAnswer.SelectedAnswerId == correctAnswer.Id)
                userCorrectAnswers++;
        }

        int totalQuestions = quiz.Questions.Count;
        var score = Math.Ceiling((userCorrectAnswers / (double)totalQuestions) * 100);

        // Create submission
        var submission = new Submission
        {
            UserId = userId,
            QuizId = quizId,
            AttemptDate = DateTime.Now,
            Score = score
        };

        db.Submissions.Add(submission);
        await db.SaveChangesAsync();

        return submission.Id;
    }
}