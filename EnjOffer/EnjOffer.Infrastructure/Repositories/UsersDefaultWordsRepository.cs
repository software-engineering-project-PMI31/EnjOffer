using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Infrastructure.Repositories
{
    public class UsersDefaultWordsRepository : IUsersDefaultWordsRepository
    {
        private readonly EnjOfferDbContext _db;

        public UsersDefaultWordsRepository(EnjOfferDbContext db)
        {
            _db = db;
        }

        public UsersDefaultWords AddUserDefaultWord(UsersDefaultWords userDefaultWord)
        {
            _db.UsersDefaultWords.Add(userDefaultWord);
            _db.SaveChanges();

            return userDefaultWord;
        }

        public bool DeleteUserDefaultWord(Guid? defaultWordId, Guid? userId)
        {
            _db.UsersDefaultWords.RemoveRange(_db.UsersDefaultWords.Where(temp => temp.DefaultWordId == defaultWordId &&
                temp.UserId == userId));
            int rowsDeleted = _db.SaveChanges();

            return rowsDeleted > 0;
        }

        public List<UsersDefaultWords> GetAllUserDefaultWords()
        {
            return _db.UsersDefaultWords.ToList();
        }

        public UsersDefaultWords? GetUserDefaultWordById(Guid? defaultWordId, Guid? userId)
        {
            return _db.UsersDefaultWords.FirstOrDefault(temp => temp.DefaultWordId == defaultWordId &&
                temp.UserId == userId);
        }

        /*public UsersDefaultWords? GetUserDefaultWordByWordAndTranslation(string word, string translation)
        {
            throw new NotImplementedException();
        }*/

        public List<UsersDefaultWords> GetUserDefaultWordsByDate(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                return _db.UsersDefaultWords.Where(temp => temp.LastTimeEntered == null).ToList();
            }

            return _db.UsersDefaultWords.Where(temp => temp.LastTimeEntered == dateTime.Value).ToList();
        }

        public UsersDefaultWords UpdateUserDefaultWord(UsersDefaultWords userDefaultWordUpdate)
        {
            UsersDefaultWords? matchingUserDefaultWord =
                _db.UsersDefaultWords.FirstOrDefault(temp => temp.DefaultWordId == userDefaultWordUpdate.DefaultWordId &&
                temp.UserId == userDefaultWordUpdate.UserId);

            if (matchingUserDefaultWord == null)
            {
                return userDefaultWordUpdate;
            }

            matchingUserDefaultWord.LastTimeEntered = userDefaultWordUpdate.LastTimeEntered;
            matchingUserDefaultWord.CorrectEnteredCount = userDefaultWordUpdate.CorrectEnteredCount;
            matchingUserDefaultWord.IncorrectEnteredCount = userDefaultWordUpdate.IncorrectEnteredCount;

            int countUpdated = _db.SaveChanges();

            return matchingUserDefaultWord;
        }
    }
}
