using System;
using System.ComponentModel.DataAnnotations;

namespace Bright_Ideas.Models
{
    public class Login : BaseEntity
    {
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        [DataType(DataType.EmailAddress)]
        public string LoginEmail { get; set; }
 
        [Required]
        [MinLength(8)]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}