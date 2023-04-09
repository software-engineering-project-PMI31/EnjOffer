using EnjOffer.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IUserWordsService
    {
        UserWordsResponse AddUserWord(UserWordsAddRequest? userWordAddRequest);
        List<UserWordsResponse> GetAllUserWords();
        List<UserWordsResponse> GetUserWordsByDate(DateTime? dateTime);
        List<UserWordsResponse> GetUserWordsSortedByPriority(List<UserWordsResponse> userWords);
        UserWordsResponse? GetUserWordById(Guid? userWordId);
        UserWordsResponse UpdateUserWord(UserWordsUpdateRequest? userWordsUpdateRequest);
        double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd);
        bool DeleteUserWord(Guid? userWordId);
    }
}
