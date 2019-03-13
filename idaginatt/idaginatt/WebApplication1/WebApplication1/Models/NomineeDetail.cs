﻿using System;
using System.Collections.Generic;
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
        public string Nominee_FirstName { get; set; }
        public string Nominee_Lastname { get; set; }
        public int Nominee_Votes { get; set; }
    }
}
