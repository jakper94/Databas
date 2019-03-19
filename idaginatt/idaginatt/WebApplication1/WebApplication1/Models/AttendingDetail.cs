using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AttendingDetail
    {
        [Key]
        public int Attending_Id { get; set; }
        
        [Required, DisplayName("Användarnamn")]
        public string Attending_User { get; set; }
        [Required, DisplayName("Förnamn")]
        public string Attending_Firstname { get; set; }
        [Required, DisplayName("Efternamn")]
        public string Attending_Lastname { get; set; }
        [Required, DisplayName("Klass")]
        public string Attending_Class { get; set; }
        [DisplayName("Allergier")]
        public string Attending_Foodpref { get; set; }
        [DisplayName("År")]
        public int Attending_Year { get; set; }

        public AttendingDetail() {}
    }
}
