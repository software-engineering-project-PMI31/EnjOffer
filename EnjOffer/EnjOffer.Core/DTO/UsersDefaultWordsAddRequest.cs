using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class UsersDefaultWordsAddRequest
    {
        [Required(ErrorMessage = $"{nameof(UserId)} can't be blank")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = $"{nameof(DefaultWordId)} can't be blank")]
        public Guid DefaultWordId { get; set; }

        public UsersDefaultWords ToUsersDefaultWords()
        {
            return new UsersDefaultWords()
            {
                UserId = UserId,
                DefaultWordId = DefaultWordId
            };
        }
    }
}
