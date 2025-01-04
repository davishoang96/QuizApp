using FastEndpoints;
using QuizApp.Common.Request;
using QuizApp.Database.Repositories;

namespace QuizApp.Endpoints.Submission;

public class SaveSubmissionEndpoint : Endpoint<SaveSubmissionRequest, int>
{
    private readonly IQuizRepository quizRepository;
    public SaveSubmissionEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
    }

    public override void Configure()
    {
        Post("submission/save");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SaveSubmissionRequest r, CancellationToken ct)
    {
        var result = await quizRepository.SaveSubmissionAsync(r.UserId, r.QuizId, r.UserAnswerDTOs);
        if (result < 0)
        {
            ThrowError("Cannot save quiz");
        }

        await SendAsync(result);
    }
}
