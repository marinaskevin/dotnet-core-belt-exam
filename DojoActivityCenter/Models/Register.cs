using System.ComponentModel.DataAnnotations;

namespace DojoActivityCenter.Models
{
    public class Register : BaseEntity
    {
        [Required]
        [MinLength(2)]
        [Display(Name="First Name")]
        [RegularExpression("^([a-zA-Z])+$", ErrorMessage = "Invalid First Name. Characters [a-z] [A-Z] only!")]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(2)]
        [Display(Name="Last Name")]
        [RegularExpression("^([a-zA-Z])+$", ErrorMessage = "Invalid Last Name. Characters [a-z] [A-Z] only!")]
        public string LastName { get; set; }
 
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
 
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation must match")]
        public string PasswordConfirmation { get; set; }

    }
}