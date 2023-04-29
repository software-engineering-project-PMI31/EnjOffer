using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.RepositoryContracts
{
    public interface IUserWordsRepository
    {
        UserWords AddUserWord(UserWords userWord);
        List<UserWords> GetAllUserWords();
        List<UserWords> GetUserWordsByDate(DateTime? dateTime);
        UserWords? GetUserWordById(Guid? userWordId);
        UserWords? GetUserWordByWordAndTranslation(string word, string translation);
        UserWords UpdateUserWord(UserWords userWordUpdate);
        bool DeleteUserWord(Guid? userWordId);
    }
}
