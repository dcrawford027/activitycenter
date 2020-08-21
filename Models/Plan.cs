using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivityCenter.Models
{
    public class Plan
    {
        [Key]
        public int PlanId {get;set;}

        [Required]
        [Display(Name = "Title:")]
        public string Title {get;set;}

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date:")]
        public DateTime Date {get;set;}

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Time:")]
        public DateTime Time {get;set;}

        [Required]
        [Display(Name = "Duration:")]
        public int DurationNumber {get;set;}

        [Required]
        public string DurationType {get;set;}

        [Required]
        [Display(Name = "Description:")]
        public string Description {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // Foreign Keys
        public int UserId {get;set;}

        // Navigation Properties
        public User Coordinator {get;set;}
        public List<Participant> PlanUser {get;set;}
    }
}