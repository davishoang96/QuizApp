using FastEndpoints;
using QuizApp.Common.Request;
using QuizApp.Database.Repositories;

namespace QuizApp.Endpoints.Question
{
    public class SaveOrUpdateQuestion : Endpoint<SaveOrUpdateQuestionRequest, int>
    {
        private readonly IQuizRepository quizRepository;
        public SaveOrUpdateQuestion(IQuizRepository quizRepository) 
        {
            this.quizRepository = quizRepository;
        }

        public override void Configure()
        {
            Post("question/savequestions");
            AllowAnonymous();
        }

        public override async Task HandleAsync(SaveOrUpdateQuestionRequest r, CancellationToken ct)
        {
            var result = await quizRepository.SaveOrUpdateQuestion(r.QuestionDTOs, r.QuizId);
            if (result < 0)
            {
                ThrowError("Cannot save all questions");
            }

            await SendAsync(result);
        }
    }
}
