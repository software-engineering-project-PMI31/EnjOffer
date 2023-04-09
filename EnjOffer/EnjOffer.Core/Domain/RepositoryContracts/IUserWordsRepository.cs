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
        Users AddUserWord(Users userWord);
        List<Users> GetAllUserWords();
        List<Users> GetUserWordsByDate(DateTime? dateTime);
        List<Users> GetUserWordsSortedByPriority(List<Users> userWords);
        Users GetUserWordById(Guid userWordId);
        Users UpdateUserWord(Users userWordUpdate);
        double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd);
        bool DeleteUserWord(Guid userWordId);
    }
}
