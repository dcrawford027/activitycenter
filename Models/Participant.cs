using System;
using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId {get;set;}

        // Foreign Keys
        public int UserId {get;set;}
        public int PlanId {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        // Navigation Parameters
        public User User {get;set;}
        public Plan Plan {get;set;}
    }
}