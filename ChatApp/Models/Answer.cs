using System;

namespace ChatApp.Models
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string CreatedBy { get; set; }
        public string Body { get; set; }
        public bool Deleted { get; set; }
    }
}