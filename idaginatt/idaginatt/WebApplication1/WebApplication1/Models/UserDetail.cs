using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class UserDetail
    {
        public UserDetail() { }
        [Key]
        public int User_Id { get; set; }
        [Required, DisplayName("Username")]
        public string User_UserName { get; set; }
        [Required, DisplayName("Password"), DataType(DataType.Password)]
        public string User_Password { get; set; }
        
        public string User_FirstName { get; set; }
        public string User_LastName { get; set; }
        public string User_Class { get; set; }

        public Boolean User_HasVoted { get; set; }
        public Boolean User_IsAdmin { get; set; }
        public string LogInErrorMessage { get; set; }
    }
}
