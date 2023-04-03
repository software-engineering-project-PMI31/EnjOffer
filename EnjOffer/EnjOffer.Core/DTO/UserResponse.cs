using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.DTO
{
    public class UserResponse
    {
        public Guid UserId { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public UserRole? Role { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(UserResponse))
            {
                return false;
            }
            UserResponse user_to_compare = (UserResponse)obj;
            return UserId == user_to_compare.UserId &&
                Email == user_to_compare.Email &&
                Password == user_to_compare.Password &&
                Role == user_to_compare.Role;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Bug", "S3249:Classes directly extending \"object\" should not call \"base\" in \"GetHashCode\" or \"Equals\"", Justification = "<Pending>")]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public static class UserResponseExtensions
    {
        public static UserResponse ToUserResponse(this Users users)
        {
            return new UserResponse()
            {
                UserId = users.UserId,
                Email = users.Email,
                Password = users.Password,
                Role = users.Role
            };
        }
    }
}
