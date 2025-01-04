using FastEndpoints;
using QuizApp.Common.DTO;
using QuizApp.Database.Repositories;

namespace QuizApp.Endpoints.Quiz;

public class CreateQuizEndpoint : Endpoint<QuizDTO, int>
{
    private readonly IQuizRepository quizRepository;
    public CreateQuizEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
    }

    public override void Configure()
    {
        Post("quiz/saveorupdatequiz");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QuizDTO dto, CancellationToken ct)
    {
        var result = await quizRepository.SaveOrUpdateQuiz(dto);
        if(result < 0)
        {
            ThrowError("Cannot save quiz");
        }

        await SendAsync(result);
    }
}
