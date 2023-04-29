using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly EnjOfferDbContext _db;

        public UsersRepository(EnjOfferDbContext db)
        {
            _db = db;
        }

        public Users AddUser(Users user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }

        public List<Users> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public Users? GetUserById(Guid userId)
        {
            return _db.Users.FirstOrDefault(temp => temp.UserId == userId);
        }

        public bool DeleteUser(Guid userId)
        {
            _db.Users.RemoveRange(_db.Users.Where(temp => temp.UserId == userId));
            int rowsDeleted = _db.SaveChanges();

            return rowsDeleted > 0;
        }

        public Users? GetUserByEmail(string email)
        {
            return _db.Users.FirstOrDefault(temp => temp.Email == email);
        }
    }
}
