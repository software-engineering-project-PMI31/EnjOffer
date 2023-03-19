using EnjOffer.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.Entities
{
    public class Users
    { 
        public Guid UserId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserRole userRole { get; set; }
    }
}
