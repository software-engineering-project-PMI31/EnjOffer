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
        List<UsersDefaultWordsResponse> GetUserDefaultWordsSortedByPriority(List<UsersDefaultWordsResponse> userDefaultWords);
        UsersDefaultWordsResponse? GetUserDefaultWordById(Guid? defaultWordId, Guid? userId);
        UsersDefaultWordsResponse UpdateUserDefaultWord(UsersDefaultWordsUpdateRequest? userWordsUpdateRequest);
        double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd);
        bool DeleteUserDefaultWord(Guid? defaultWordId, Guid? userId);
    }
}
