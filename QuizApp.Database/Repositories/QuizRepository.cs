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

    public async Task<int> CreateQuestion(int quizId, string question)
    {
        var quizModel = db.Quizzes.SingleOrDefault(x => x.Id == quizId);
        if (quizModel is null)
        {
            throw new InvalidOperationException($"Cannot found quiz id = {quizId}");
        }

        var questionModel = new Question
        {
            QuizId = quizModel.Id,
            Quiz = quizModel,    
            Title = question,
        };
        
        var result = db.Questions.Add(questionModel);
        
        await db.SaveChangesAsync();
        
        return result.Entity.Id;
    }
}