using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Infrastructure.Repositories
{
    public class DefaultWordsRepository : IDefaultWordsRepository
    {
        private readonly EnjOfferDbContext _db;

        public DefaultWordsRepository(EnjOfferDbContext db)
        {
            _db = db;
        }

        public DefaultWords AddDefaultWord(DefaultWords defaultWord)
        {
            _db.DefaultWords.Add(defaultWord);
            _db.SaveChanges();

            return defaultWord;
        }

        public bool DeleteDefaultWord(Guid defaultWordId)
        {
            _db.DefaultWords.RemoveRange(_db.DefaultWords.Where(temp => temp.DefaultWordId == defaultWordId));
            int rowsDeleted = _db.SaveChanges();

            return rowsDeleted > 0;
        }

        public List<DefaultWords> GetAllDefaultWords()
        {
            return _db.DefaultWords.ToList();
        }

        public DefaultWords? GetDefaultWordById(Guid defaultWordId)
        {
            return _db.DefaultWords.FirstOrDefault(temp => temp.DefaultWordId == defaultWordId);
        }

        public DefaultWords? GetDefaultWordByWordAndTranslation(string word, string translation)
        {
            return _db.DefaultWords.FirstOrDefault(temp => temp.Word == word && temp.WordTranslation == translation);
        }
    }
}
