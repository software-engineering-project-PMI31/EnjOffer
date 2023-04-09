using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.RepositoryContracts
{
    public interface IUsersRepository
    {
        Users AddUser(Users user);
        List<Users> GetAllUsers();
        Users GetUserById(Guid userId);
        bool DeleteUser(Guid userId);
    }
}
