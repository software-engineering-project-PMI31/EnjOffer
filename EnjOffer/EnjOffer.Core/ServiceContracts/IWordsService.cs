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
        List<WordsResponse> GetWordsSortedByPriority(IEnumerable<WordsResponse> userWordsDefaultWords);
        WordsResponse GetWordToCheck();
        WordsResponse UpdateWord(WordsUpdateRequest? wordUpdateRequest);
        public IEnumerable<WordsResponse> JoinDefaultWords();
        public IEnumerable<WordsResponse> JoinDefaultWords(List<DefaultWordResponse> defaultWords,
            List<UsersDefaultWordsResponse> usersDefaultWords);
        public WordsResponse GetNextWordToCheck();
        bool CheckWord(string word);
    }
}
