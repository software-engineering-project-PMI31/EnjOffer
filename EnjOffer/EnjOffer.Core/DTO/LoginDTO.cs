using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = $"{nameof(Email)} can't be blank")]
        [EmailAddress(ErrorMessage = $"{nameof(Email)} should be entered in a proper format")]
        public string Email { get; set; }

        [Required(ErrorMessage = $"{nameof(Password)} can't be blank")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
