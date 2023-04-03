using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class UserAddRequest
    {
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email isn't in valid format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "UserRole can't be blank")]
        public UserRole Role { get; set; }

        public Users ToUser()
        {
            return new Users()
            {
                Email = Email,
                Password = Password,
                Role = Role,
            };
        }
    }
}
