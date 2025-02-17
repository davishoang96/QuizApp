﻿using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using QuizApp.Common.DTO;
using QuizApp.Database.Repositories;

namespace BlazorQuizApp.Endpoints.Question;

[HttpGet("question/getallquestions"), Authorize]
public class GetAllQuestionsEndpoint : Endpoint<EmptyRequest, IEnumerable<QuestionDTO>>
{
    private readonly IQuizRepository quizRepository;
    public GetAllQuestionsEndpoint(IQuizRepository quizRepository) 
    {
        this.quizRepository = quizRepository;
    }

    public override async Task HandleAsync(EmptyRequest r, CancellationToken ct)
    {
        var result = await quizRepository.GetQuestions();
        if (result == null)
        {
            ThrowError("Cannot get all questions");
        }

        await SendAsync(result);
    }
}
