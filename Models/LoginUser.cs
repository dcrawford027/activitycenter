using System.ComponentModel.DataAnnotations;

namespace ActivityCenter.Models
{
    public class LoginUser
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string LoginEmail {get;set;}

        [Required]
        [MinLength(8, ErrorMessage = "Your password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string LoginPassword {get;set;}
    }
}