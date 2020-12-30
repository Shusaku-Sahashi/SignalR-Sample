using System;
using System.Collections.Generic;
using System.Threading;

namespace ChatApp.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public string Title{ get; set; }
        public string Body { get; set; }
        public int Score { get; set; }
        public string CreatedBy { get; set; }
        public List<Answer> Answers { get; set; }
        public bool Deleted { get; set; }
    }
}