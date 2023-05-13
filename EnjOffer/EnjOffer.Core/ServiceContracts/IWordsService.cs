using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IWordsService
    {
        WordsResponse? GetWordById(Guid? defaultWordId, Guid? userWordId, Guid userId);
        List<WordsResponse> GetWordsSortedByPriority(IEnumerable<WordsResponse> userWordsDefaultWords);
        WordsResponse GetWordToCheck(Guid userId);
        WordsResponse UpdateWord(WordsUpdateRequest? wordUpdateRequest);
        public IEnumerable<WordsResponse> JoinDefaultWords();
        public IEnumerable<WordsResponse> JoinDefaultWords(List<DefaultWordResponse> defaultWords,
            List<UsersDefaultWordsResponse> usersDefaultWords);
        public IEnumerable<WordsResponse> JoinDefaultWordsAndUserWords(Guid userId);
        public WordsResponse GetNextWordToCheck(string word, Guid userId);
        bool CheckWord(string word, Guid userId);
    }
}
