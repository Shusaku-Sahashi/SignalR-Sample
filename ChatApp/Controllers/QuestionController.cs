using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        // https://docs.microsoft.com/ja-jp/aspnet/core/signalr/hubcontext?view=aspnetcore-5.0
        // Hub意外から送信する場合は、IHubContextを使用する。(今回はHubでの型を強制しているので、ジェネリクスは２つとる。）
        private readonly IHubContext<QuestionHub, IQuestionHub> _hubContext;

        private static ConcurrentBag<Question> questions = new ConcurrentBag<Question>()
        {
          new Question {
                Id = Guid.Parse("b00c58c0-df00-49ac-ae85-0a135f75e01b"),
                CreatedBy = "terry.pratchett@lspace.com",
                Title = "Welcome",
                Body = "Welcome to the _mini Stack Overflow_ rip-off!\n" +
                       "This application was built as an example on how **SignalR** and **Vue** can play together\n" +
                       " - [Original article in the DotNetCurry magazine](https://www.dotnetcurry.com/aspnet-core/1480/aspnet-core-vuejs-signalr-app)\n" +
                       " - [GitHub source of this app](https://github.com/DaniJG/so-signalr)",
                Answers = new List<Answer>{ new Answer { Body = "Sample answer", CreatedBy = "pierre.lemaitre@gmail.com" }}
            },
          new Question {
                Id = Guid.Parse("eb20d554-80be-429c-8418-5a72245bcaf3"),
                CreatedBy = "terry.pratchett@lspace.com",
                Title = "Welcome Back!",
                Body = "The second iteration enhanced the app adding authentication.\n" +
                       "It includes examples for both **cookie** and **jwt** based authentication integrated with Vue and SignalR.\n" +
                       "While this will be the subject of a new DotNetCurry article, you can Start by checking out these links:\n" +
                       " - [SignalR authentication docs](https://docs.microsoft.com/en-us/aspnet/core/signalr/authn-and-authz?view=aspnetcore-2.2)\n" +
                       " - [Example with multiple authentication schemes](https://github.com/aspnet/AspNetCore/tree/release/2.2/src/Security/samples/PathSchemeSelection)\n" +
                       " - [JWT examples with ASP.NET Core](https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api)\n" +
                       " - [Securing APIs in ASP.NET Core](https://www.blinkingcaret.com/2018/07/18/secure-an-asp-net-core-web-api-using-cookies/)",
                Answers = new List<Answer>()
            },
        };

        public QuestionController(IHubContext<QuestionHub, IQuestionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public IEnumerable GetQuestions()
        {
            return questions.Where(t => !t.Deleted).Select(q => new
            {
                q.Id,
                q.CreatedBy,
                q.Title,
                q.Body,
                q.Score,
                AnswersCount = q.Answers.Count,
            });
        }

        [HttpGet("{id}")]
        public ActionResult GetQuestion(Guid id)
        {
            var question = questions.SingleOrDefault(q => q.Id == id);
            if (question == null) return NotFound();

            return new JsonResult(question);
        }

        [HttpPost]
        [Authorize]
        public async Task<Question> AddQuestion([FromBody] Question input)
        {
            var question = new Question()
            {
                Id = Guid.NewGuid(),
                CreatedBy = User?.Identity?.Name,
                Answers = new List<Answer>(),
                Body = input.Body,
                Title = input.Title,
                Score = input.Score,
            };
            questions.Add(question);

            await _hubContext.Clients.All.QuestionAdded(question);

            return question;
        }

        [HttpPost("{id}/answer")]
        [Authorize]
        public async Task<ActionResult> AddAnswerAsync(Guid id, [FromBody] Answer input)
        {
            var question = questions.SingleOrDefault(t => t.Id == id && !t.Deleted);
            if (question == null) return NotFound();

            var answer = new Answer()
            {
                Id = Guid.NewGuid(),
                QuestionId = id,
                CreatedBy = User?.Identity?.Name,
                Body = input.Body
            };
            question.Answers.Add(answer);

            await _hubContext.Clients.Group(id.ToString()).AnswerAdded(answer);
            await _hubContext.Clients.All.AnswerCountChange(id, question.Answers.Count);

            return new JsonResult(answer);
        }

        [HttpPost("{id}/upvote")]
        [Authorize]
        public async Task<ActionResult> UpvoteQuestionAsync(Guid id)
        {
            var question = questions.SingleOrDefault(t => t.Id == id && !t.Deleted);
            if (question == null) return NotFound();

            question.Score++;

            await _hubContext.Clients.All.QuestionScoreChange(id, question.Score);
            return new JsonResult(question);
        }

        [HttpPost("{id}/downvote")]
        [Authorize]
        public async Task<ActionResult> DownvoteQuestionAsync(Guid id)
        {
            var question = questions.SingleOrDefault(t => t.Id == id && !t.Deleted);
            if (question == null) return NotFound();

            question.Score--;

            await _hubContext.Clients.All.QuestionScoreChange(id, question.Score);
            return new JsonResult(question);
        }
    }
}