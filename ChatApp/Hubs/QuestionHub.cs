using System;
using System.Threading.Tasks;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public interface IQuestionHub
    {
        Task QuestionAdded(Question question);
        Task AnswerAdded(Answer answer);
        Task AnswerCountChange(Guid id, int answerCount);
        Task QuestionScoreChange(Guid id, int score);
    }

    [Authorize]
    public class QuestionHub : Hub<IQuestionHub>
    {
        // These 2 methods will be called from the client
        public async Task JoinQuestionGroup(Guid id) => 
            await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());

        public async Task LeaveQuestionGroup(Guid id) =>
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, id.ToString());
    }
}