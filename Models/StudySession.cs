using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flashcards.Models
{
    public class StudySession
    {
        public int SessionId { get; set; }
        public int StackId { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}