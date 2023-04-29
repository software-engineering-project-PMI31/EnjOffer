using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Infrastructure.Repositories
{
    public class UserWordsRepository : IUserWordsRepository
    {
        private readonly EnjOfferDbContext _db;

        public UserWordsRepository(EnjOfferDbContext db)
        {
            _db = db;
        }

        public UserWords AddUserWord(UserWords userWord)
        {
            _db.UserWords.Add(userWord);
            _db.SaveChanges();

            return userWord;
        }

        public bool DeleteUserWord(Guid? userWordId)
        {
            _db.UserWords.RemoveRange(_db.UserWords.Where(temp => temp.UserWordId == userWordId));
            int rowsDeleted = _db.SaveChanges();

            return rowsDeleted > 0;
        }

        public List<UserWords> GetAllUserWords()
        {
            return _db.UserWords.ToList();
        }

        public UserWords? GetUserWordById(Guid? userWordId)
        {
            return _db.UserWords.FirstOrDefault(temp => temp.UserWordId == userWordId);
        }

        public UserWords? GetUserWordByWordAndTranslation(string word, string translation)
        {
            return _db.UserWords.FirstOrDefault(temp => temp.Word == word && temp.WordTranslation == translation);
        }

        public List<UserWords> GetUserWordsByDate(DateTime? dateTime)
        {
            if (dateTime is null)
            {
                return _db.UserWords.Where(temp => temp.LastTimeEntered == null).ToList();
            }

            return _db.UserWords.Where(temp => temp.LastTimeEntered == dateTime.Value).ToList();
        }

        public UserWords UpdateUserWord(UserWords userWordUpdate)
        {
            UserWords? matchingUserWord = _db.UserWords.FirstOrDefault(temp => temp.UserWordId == userWordUpdate.UserWordId);

            if (matchingUserWord == null)
            {
                return userWordUpdate;
            }

            matchingUserWord.LastTimeEntered = userWordUpdate.LastTimeEntered;
            matchingUserWord.CorrectEnteredCount = userWordUpdate.CorrectEnteredCount;
            matchingUserWord.IncorrectEnteredCount = userWordUpdate.IncorrectEnteredCount;

            int countUpdated = _db.SaveChanges();

            return matchingUserWord;
        }
    }
}
