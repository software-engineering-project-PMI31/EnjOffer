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
        WordsResponse? GetWordById(Guid? defaultWordId, Guid? userWordId);
        List<WordsResponse> GetWordsSortedByPriority(IEnumerable<WordsResponse> userWordsDefaultWords);
        WordsResponse GetWordToCheck();
        WordsResponse UpdateWord(WordsUpdateRequest? wordUpdateRequest);
        public IEnumerable<WordsResponse> JoinDefaultWords();
        public IEnumerable<WordsResponse> JoinDefaultWords(List<DefaultWordResponse> defaultWords,
            List<UsersDefaultWordsResponse> usersDefaultWords);
        public IEnumerable<WordsResponse> JoinDefaultWordsAndUserWords();
        public WordsResponse GetNextWordToCheck(string word);
        bool CheckWord(string word);
    }
}
