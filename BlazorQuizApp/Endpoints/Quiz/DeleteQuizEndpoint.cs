using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Common.Request;
using QuizApp.Database.Repositories;

namespace BlazorQuizApp.Endpoints.Quiz;

[HttpDelete("quiz/delete"), Authorize]
public class DeleteQuizEndpoint : Endpoint<DeleteQuizRequest, bool>
{
    private readonly IQuizRepository quizRepository;
    public DeleteQuizEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
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
