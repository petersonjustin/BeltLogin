using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BeltLogin.Models;
using System.Collections.Generic;
using System.Linq;

namespace BeltLogin.Models
{
    public class ValidLoginUser: BaseEntity
    {

        [Required]
        [EmailAddress]
        [MinLength(8)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}