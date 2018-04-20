using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BeltLogin.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeltLogin.Models
{
    public class ValidRegUser: BaseEntity
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression(@"^[0-9A-Za-z ]+")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [RegularExpression(@"^[0-9A-Za-z ]+")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(8)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,}$", ErrorMessage = "PW needs one letter, one number and one special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string confirm_Password { get; set; }
    }
}