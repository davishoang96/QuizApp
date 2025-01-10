using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Common.Request;
using QuizApp.Common.Response;
using QuizApp.Database.Repositories;

namespace BlazorQuizApp.Endpoints.Submission;

[HttpPost("submission/save"), Authorize]
public class SaveSubmissionEndpoint : Endpoint<SaveSubmissionRequest, SaveSubmissionResponse>
{
    private readonly IQuizRepository quizRepository;
    public SaveSubmissionEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
    }

    public override async Task HandleAsync(SaveSubmissionRequest r, CancellationToken ct)
    {
        var score = await quizRepository.SaveSubmissionAsync(r.UserId, r.QuizId, r.UserAnswerDTOs);
        if (score < 0)
        {
            ThrowError("Cannot save quiz");
        }

        await SendAsync(new SaveSubmissionResponse { Score = score });
    }
}
