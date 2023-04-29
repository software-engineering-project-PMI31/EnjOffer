using EnjOffer.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Domain.RepositoryContracts
{
    public interface IDefaultWordsRepository
    {
        DefaultWords AddDefaultWord(DefaultWords defaultWord);
        List<DefaultWords> GetAllDefaultWords();
        DefaultWords? GetDefaultWordById(Guid defaultWordId);
        DefaultWords? GetDefaultWordByWordAndTranslation(string word, string translation);
        bool DeleteDefaultWord(Guid defaultWordId);
    }
}
