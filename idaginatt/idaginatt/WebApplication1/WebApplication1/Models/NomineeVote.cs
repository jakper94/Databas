using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class NomineeVote
    {
        public NomineeVote() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AmountOfVotes { get; set; }
    }
}
