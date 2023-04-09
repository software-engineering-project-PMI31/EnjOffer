using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Services
{
    public class UsersDefaultWordsService : IUsersDefaultWordsService
    {
        public UsersDefaultWordsResponse AddUserDefaultWord()
        {
            throw new NotImplementedException();
        }

        public UsersDefaultWordsResponse AddUserDefaultWord(UsersDefaultWordsAddRequest? userDefaultWordAddRequest)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUserDefaultWord(Guid? userWordId)
        {
            throw new NotImplementedException();
        }

        public List<UsersDefaultWordsResponse> GetAllUserDefaultWords()
        {
            throw new NotImplementedException();
        }

        public double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd)
        {
            throw new NotImplementedException();
        }

        public UsersDefaultWordsResponse? GetUserDefaultWordById(Guid? userDefaultWordId)
        {
            throw new NotImplementedException();
        }

        public List<UsersDefaultWordsResponse> GetUserDefaultWordsByDate(DateTime? dateTime)
        {
            throw new NotImplementedException();
        }

        public List<UsersDefaultWordsResponse> GetUserDefaultWordsSortedByPriority(List<UserWordsResponse> userDefaultWords)
        {
            throw new NotImplementedException();
        }

        public UsersDefaultWordsResponse UpdateUserDefaultWord(UserWordsUpdateRequest? userWordsUpdateRequest)
        {
            throw new NotImplementedException();
        }
    }
}
