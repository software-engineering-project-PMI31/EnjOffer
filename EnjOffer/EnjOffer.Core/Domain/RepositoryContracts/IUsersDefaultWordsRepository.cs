using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.RepositoryContracts
{
    public interface IUsersDefaultWordsRepository
    {
        UsersDefaultWords AddUserDefaultWord(UsersDefaultWords userDefaultWord);
        List<UsersDefaultWords> GetAllUserDefaultWords();
        List<UsersDefaultWords> GetUserDefaultWordsByDate(DateTime? dateTime);
        UsersDefaultWords? GetUserDefaultWordById(Guid? defaultWordId, Guid? userId);
        //UsersDefaultWords? GetUserDefaultWordByWordAndTranslation(string word, string translation);
        UsersDefaultWords UpdateUserDefaultWord(UsersDefaultWords userWordUpdate);
        bool DeleteUserDefaultWord(Guid? defaultWordId, Guid? userId);
    }
}
