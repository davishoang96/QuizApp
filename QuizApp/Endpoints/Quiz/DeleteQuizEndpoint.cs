using FastEndpoints;
using QuizApp.Common.Request;
using QuizApp.Database.Repositories;

namespace QuizApp.Endpoints.Quiz;

public class DeleteQuizEndpoint : Endpoint<DeleteQuizRequest, bool>
{
    private readonly IQuizRepository quizRepository;
    public DeleteQuizEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
    }

    public override void Configure()
    {
        Delete("quiz/delete");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteQuizRequest r, CancellationToken ct)
    {
        var result = await quizRepository.DeleteQuiz(r.QuizId);
        if(!result)
        {
            ThrowError("Cannot delete quiz");
        }

        await SendAsync(result);
    }
}
