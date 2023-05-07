using EnjOffer.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IUsersService
    {
        UserResponse AddUser(UserAddRequest? userAddRequest);
        List<UserResponse> GetAllUsers();
        UserResponse? GetUserById(Guid? userId);
        bool DeleteUser(Guid? userId);
        bool DeleteUserByEmail(string email);
    }
}
