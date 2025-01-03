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

    public async Task<int> CreateQuiz(string name)
    {
        var model = db.Add(new Quiz
        {
            QuizName = name,
        });

        await db.SaveChangesAsync();
        
        return model.Entity.Id;
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
                Answers = q.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            })
            .ToListAsync();
    }
}