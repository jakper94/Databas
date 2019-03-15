using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class NomineeDetail
    {
        public NomineeDetail() {}
        [Key]
        public int Nominee_Id { get; set; }
        [DisplayName ("Förnamn")]
        public string Nominee_FirstName { get; set; }
        [DisplayName("Efternamn")]
        public string Nominee_LastName { get; set; }
        public string Nominee_ImgLink { get; set; }
        public int Nominee_Votes { get; set; }
        [DisplayName("År")]

        public int Nominee_Year { get; set; }
    }
}
