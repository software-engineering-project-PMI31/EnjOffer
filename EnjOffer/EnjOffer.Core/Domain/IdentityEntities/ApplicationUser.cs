using EnjOffer.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ICollection<UserWords>? UserWords { get; set; }
        public ICollection<UserStatistics>? UserStatistics { get; set; }
        public ICollection<UsersDefaultWords>? UsersDefaultWords { get; set; }
        public ICollection<DefaultWords>? DefaultWords { get; set; }
    }
}
