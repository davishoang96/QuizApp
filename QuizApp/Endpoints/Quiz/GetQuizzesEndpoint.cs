using FastEndpoints;
using QuizApp.Common.DTO;
using QuizApp.Database.Repositories;

namespace QuizApp.Endpoints.Quiz;

public class GetQuizzesEndpoint : Endpoint<EmptyRequest, IEnumerable<QuizDTO>>
{
    private readonly IQuizRepository quizRepository;
    public GetQuizzesEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
    }

    public override void Configure()
    {
        Get("quiz/getquizzes");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest r, CancellationToken ct)
    {
        var result = await quizRepository.GetAllQuizzes();
        if (result == null)
        {
            ThrowError("Cannot get all quizzes");
        }

        await SendAsync(result);
    }
}
