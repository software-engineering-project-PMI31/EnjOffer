using EnjOffer.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = $"{nameof(Email)} can't be blank")]
        [EmailAddress(ErrorMessage = $"{nameof(Email)} should be entered in a proper format")]
        public string Email { get; set; }

        [Required(ErrorMessage = $"{nameof(Password)} can't be blank")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = $"{nameof(RepeatedPassword)} can't be blank")]
        [MinLength(6)]
        [Compare($"{nameof(Password)}", ErrorMessage = "Password and confirm password do not match")]
        public string RepeatedPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
    }
}
