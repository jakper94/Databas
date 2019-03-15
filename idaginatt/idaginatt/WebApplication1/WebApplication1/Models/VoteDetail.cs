using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class VoteDetail
    {
        public VoteDetail() { }
        public int Vote_Id { get; set; }
        [DisplayName("Motivering")]
        public string Vote_Motivation { get; set; }
        public int Vote_Nominee { get; set; }
    }
}
