﻿using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Common.DTO;
using QuizApp.Database.Repositories;

namespace BlazorQuizApp.Endpoints.Quiz;

[HttpGet("quiz/getquizzes"), Authorize]
public class GetQuizzesEndpoint : Endpoint<EmptyRequest, IEnumerable<QuizDTO>>
{
    private readonly IQuizRepository quizRepository;
    public GetQuizzesEndpoint(IQuizRepository quizRepository)
    {
        this.quizRepository = quizRepository;
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
