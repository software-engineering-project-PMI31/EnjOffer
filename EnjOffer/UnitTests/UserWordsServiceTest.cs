using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class UserWordsServiceTest
    {
        private readonly IUserWordsService _userWordsService;

        public UserWordsServiceTest()
        {
            _userWordsService = new UserWordsService();
        }

        #region AddUserWord

        [Fact]
        public void AddUserWord_NullUserWord()
        {
            //Arrange
            UserWordsAddRequest? userWordAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                _userWordsService.AddUserWord(userWordAddRequest);
            });
        }

        [Fact]
        public void AddUserWord_WordDetailsIsNull()
        {
            //Arrange
            UserWordsAddRequest? userWordAddRequest = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = Guid.NewGuid()
            };

            //Act
            UserWordsResponse userWord_response_from_add =
                _userWordsService.AddUserWord(userWordAddRequest);
            List<UserWordsResponse> userWords_list = _userWordsService.GetAllUserWords();

            //Assert
            Assert.True(userWord_response_from_add.UserWordId != Guid.Empty);
            Assert.Contains(userWord_response_from_add, userWords_list);
        }

        [Fact]
        public void AddUsertWord_DuplicateValues()
        {
            //Arrange
            Guid guid = Guid.NewGuid();
            UserWordsAddRequest? request1 = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = guid
                
            };
            UserWordsAddRequest? request2 = new UserWordsAddRequest()
            {
                Word = "Something",
                WordTranslation = "Щось",
                UserId = guid
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _userWordsService.AddUserWord(request1);
                _userWordsService.AddUserWord(request2);
            });
        }

        #endregion

    }
}
