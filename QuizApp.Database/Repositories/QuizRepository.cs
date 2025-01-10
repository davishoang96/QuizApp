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

    public async Task<int> SaveOrUpdateQuestion(QuestionDTO questionDto)
    {
        var quizModel = db.Quizzes.SingleOrDefault(x => x.Id == questionDto.QuizId);
        if (quizModel is null)
        {
            throw new InvalidOperationException($"Cannot find quiz with id = {questionDto.QuizId}");
        }

        Question questionModel;

        if (questionDto.Id > 0)
        {
            // Update existing question
            questionModel = db.Questions.Include(q => q.Answers).SingleOrDefault(q => q.Id == questionDto.Id);
            if (questionModel is null)
            {
                throw new InvalidOperationException($"Cannot find question with id = {questionDto.Id}");
            }

            questionModel.Title = questionDto.Title;

            // Update or remove existing answers
            var existingAnswers = questionModel.Answers.ToList();
            foreach (var existingAnswer in existingAnswers)
            {
                var answerDto = questionDto.Answers.SingleOrDefault(a => a.Id == existingAnswer.Id);
                if (answerDto != null)
                {
                    existingAnswer.Text = answerDto.Text;
                    existingAnswer.IsCorrect = answerDto.IsCorrect;
                }
                else
                {
                    db.Answers.Remove(existingAnswer);
                }
            }

            // Add new answers
            var newAnswers = questionDto.Answers.Where(a => a.Id == 0).ToList();
            foreach (var newAnswerDto in newAnswers)
            {
                var newAnswer = new Answer
                {
                    Text = newAnswerDto.Text,
                    IsCorrect = newAnswerDto.IsCorrect,
                    Question = questionModel
                };
                questionModel.Answers.Add(newAnswer);
            }
        }
        else
        {
            // Create new question
            questionModel = new Question
            {
                QuizId = quizModel.Id,
                Quiz = quizModel,
                Title = questionDto.Title,
                Answers = questionDto.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            db.Questions.Add(questionModel);
        }

        await db.SaveChangesAsync();

        return questionModel.Id;
    }

    public async Task<int> SaveOrUpdateQuestion(IEnumerable<QuestionDTO> questionDtos, int quizId)
    {
        var savedQuestionIds = new List<int>();

        var quizModel = db.Quizzes.SingleOrDefault(x => x.Id == quizId);
        if (quizModel is null)
        {
            throw new InvalidOperationException($"Cannot find quiz with id = {quizId}");
        }

        foreach (var questionDto in questionDtos)
        {
            Question questionModel;

            if (questionDto.Id > 0)
            {
                // Update existing question
                questionModel = db.Questions.Include(q => q.Answers).SingleOrDefault(q => q.Id == questionDto.Id);
                if (questionModel is null)
                {
                    throw new InvalidOperationException($"Cannot find question with id = {questionDto.Id}");
                }

                questionModel.Title = questionDto.Title;

                // Update or remove existing answers
                var existingAnswers = questionModel.Answers.ToList();
                foreach (var existingAnswer in existingAnswers)
                {
                    var answerDto = questionDto.Answers.SingleOrDefault(a => a.Id == existingAnswer.Id);
                    if (answerDto != null)
                    {
                        existingAnswer.Text = answerDto.Text;
                        existingAnswer.IsCorrect = answerDto.IsCorrect;
                    }
                    else
                    {
                        db.Answers.Remove(existingAnswer);
                    }
                }

                // Add new answers
                var newAnswers = questionDto.Answers.Where(a => a.Id == 0).ToList();
                foreach (var newAnswerDto in newAnswers)
                {
                    var newAnswer = new Answer
                    {
                        Text = newAnswerDto.Text,
                        IsCorrect = newAnswerDto.IsCorrect,
                        Question = questionModel
                    };
                    questionModel.Answers.Add(newAnswer);
                }
            }
            else
            {
                // Create new question
                questionModel = new Question
                {
                    QuizId = quizModel.Id,
                    Quiz = quizModel,
                    Title = questionDto.Title,
                    Answers = questionDto.Answers.Select(a => new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                };

                db.Questions.Add(questionModel);
            }

            await db.SaveChangesAsync();

            savedQuestionIds.Add(questionModel.Id);
        }

        return savedQuestionIds.Count;
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

        return (int)submission.Score;
    }

    public async Task<IEnumerable<QuizDTO>> GetAllQuizzes()
    {
        return await db.Quizzes.Select(q => new QuizDTO
        {
            Id = q.Id,
            Description = q.Description,
            Name = q.QuizName,
        }).ToListAsync();
    }

    public async Task<bool> DeleteQuiz(int quizId)
    {
        var quiz = await db.Quizzes.FindAsync(quizId);
        if (quiz == null)
        {
            return false;
        }

        db.Quizzes.Remove(quiz);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<QuestionDTO>> GetQuestionsByQuizId(int quizId)
    {
        var questions = await db.Questions.Where(q => q.QuizId == quizId)
            .Include(q => q.Answers)
            .ToListAsync();

        if (questions.Count == 0)
        {
            return Enumerable.Empty<QuestionDTO>();
        }

        return questions.Select(q => new QuestionDTO
        {
            Id = q.Id,
            QuizId = quizId,
            Title = q.Title,
            Answers = q.Answers.Select(a => new AnswerDTO
            {
                Id = a.Id,
                Text = a.Text,
                QuestionId = q.Id,
                IsCorrect = a.IsCorrect
            }).ToList()
        });
    }
}