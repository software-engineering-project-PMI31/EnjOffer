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
        List<UsersDefaultWords> GetUserDefaultWordsSortedByPriority(List<UsersDefaultWords> userDefaultWords);
        UsersDefaultWords GetUserDefaultWordById(Guid userDefaultWordId);
        UsersDefaultWords UpdateUserDefaultWord(UsersDefaultWords userWordUpdate);
        double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd);
        bool DeleteUserDefaultWord(Guid userWordId);
    }
}
