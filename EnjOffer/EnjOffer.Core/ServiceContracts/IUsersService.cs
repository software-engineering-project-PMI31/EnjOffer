using EnjOffer.Core.Domain.IdentityEntities;
using EnjOffer.Core.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IUsersService
    {
        //UserResponse AddUser(UserAddRequest? userAddRequest);
        //List<UserResponse> GetAllUsers();
        Task<List<ApplicationUser>> GetAllUsers();
        //UserResponse? GetUserById(Guid? userId);
        //bool DeleteUser(Guid? userId);
        //bool DeleteUserByEmail(string email);
    }
}
