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

        public UserRole Role { get; set; }

        public ICollection<Books>? Books { get; set; }

        public ICollection<UsersBooks>? UsersBooks { get; set; }

        public ICollection<Advice>? Advice { get; set; }

        public ICollection<UsersAdvice>? UsersAdvice { get; set; }

        public ICollection<UsersDefaultWords>? UsersDefaultWords { get; set; }

        public ICollection<DefaultWords>? DefaultWords { get; set; }

        public ICollection<UserWords>? UserWords { get; set; }

        public ICollection<UserStatistics>? UserStatistics { get; set; }


    }
}
