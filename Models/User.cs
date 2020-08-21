using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ActivityCenter.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}

        [Required]
        [MinLength(2, ErrorMessage = "You must enter at least 2 characters.")]
        public string Name {get;set;}

        [EmailAddress]
        [Required]
        public string Email {get;set;}

        [Required]
        [MinLength(8, ErrorMessage = "Your password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password {get;set;}

        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm PW")]
        public string Confirm {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // Navigation Parameters
        public List<Plan> CreatedPlans {get;set;}
        public List<Participant> UserPlan {get;set;}
    }
}