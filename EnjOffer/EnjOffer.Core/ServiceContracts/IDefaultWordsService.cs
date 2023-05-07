using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IDefaultWordsService
    {
        DefaultWordResponse AddDefaultWord(DefaultWordAddRequest? defaultWordAddRequest);
        List<DefaultWordResponse> GetAllDefaultWords();
        DefaultWordResponse? GetDefaultWordById(Guid? defaultWordId);
        bool DeleteDefaultWordByWordAndTranslation(string word, string wordTranslation);
        bool DeleteDefaultWord(Guid? defaultWordId);

    }
}
