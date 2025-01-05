﻿using FastEndpoints;
using QuizApp.Common.DTO;
using QuizApp.Common.Request;
using QuizApp.Database.Repositories;

namespace QuizApp.Endpoints.Question;

public class GetQuestionsByQuizIdEndpoint : Endpoint<GetQuestionByQuizIdRequest, IEnumerable<QuestionDTO>>
{
    private readonly IQuizRepository quizRepository;
    public GetQuestionsByQuizIdEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
    }

    public override void Configure()
    {
        Get("question/getquestionbyquizid");
    }

    public override async Task HandleAsync(GetQuestionByQuizIdRequest r, CancellationToken ct)
    {
        var result = await quizRepository.GetQuestionsByQuizId(r.QuizId);
        if (result == null)
        {
            ThrowError("Cannot get all questions");
        }

        await SendAsync(result);
    }
}
