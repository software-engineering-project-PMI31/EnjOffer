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
        List<UserWords> GetUserWordsSortedByPriority(List<UserWords> userWords);
        UserWords GetUserWordById(Guid? userWordId);
        UserWords UpdateUserWord(UserWords userWordUpdate);
        double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd);
        bool DeleteUserWord(Guid? userWordId);
    }
}
