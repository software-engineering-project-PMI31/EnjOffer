using EnjOffer.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IUsersDefaultWordsService
    {
        UsersDefaultWordsResponse AddUserDefaultWord(UsersDefaultWordsAddRequest? userDefaultWordAddRequest);
        List<UsersDefaultWordsResponse> GetAllUserDefaultWords();
        List<UsersDefaultWordsResponse> GetUserDefaultWordsByDate(DateTime? dateTime);
        List<UsersDefaultWordsResponse> GetUserDefaultWordsSortedByPriority(List<UserWordsResponse> userDefaultWords);
        UsersDefaultWordsResponse? GetUserDefaultWordById(Guid? userDefaultWordId);
        UsersDefaultWordsResponse UpdateUserDefaultWord(UserWordsUpdateRequest? userWordsUpdateRequest);
        double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd);
        bool DeleteUserDefaultWord(Guid? userWordId);
    }
}
