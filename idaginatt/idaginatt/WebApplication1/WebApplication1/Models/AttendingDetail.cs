using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class AttendingDetail
    {
        [Key]
        public int Attending_Id { get; set; }
        [Required]
        public string Attending_User { get; set; }
        public string Attending_Foodpref { get; set; }
        public int Attending_Year { get; }

        public AttendingDetail()
        {
            Attending_Year = DateTime.Now.Year;
        }


    }
}
