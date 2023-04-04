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
    }
}
