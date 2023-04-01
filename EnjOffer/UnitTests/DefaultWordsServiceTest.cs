using Xunit;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using System;
using EnjOffer.Core.Services;
using System.Collections.Generic;

namespace UnitTests
{
    public class DefaultWordsServiceTest
    {
        private readonly IDefaultWordsService _defaultWordsService;

        public DefaultWordsServiceTest()
        {
            _defaultWordsService = new DefaultWordsService();
        }

        #region AddDefaultWord
        [Fact]
        public void AddDefaultWord_NullDefaultWordDetails()
        {
            //Arrange
            DefaultWordAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                // Act
                _defaultWordsService.AddDefaultWord(request);
            });
        }

        [Fact]
        public void AddDefaultWord_NullWordProperty()
        {
            //Arrange
            DefaultWordAddRequest? request = new DefaultWordAddRequest()
            {
                Word = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _defaultWordsService.AddDefaultWord(request);
            });
        }

        [Fact]
        public void AddDefaultWord_NullTranslationProperty()
        {
            //Arrange
            DefaultWordAddRequest? request = new DefaultWordAddRequest()
            {
                WordTranslation = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _defaultWordsService.AddDefaultWord(request);
            });
        }

        [Fact]
        public void AddDefaultWord_DuplicateValues()
        {
            //Arrange
            DefaultWordAddRequest? request1 = new DefaultWordAddRequest()
            {
                Word = "Something",
                WordTranslation = "ўось",
                ImageSrc = "imgNotFound.png"
            };
            DefaultWordAddRequest? request2 = new DefaultWordAddRequest()
            {
                Word = "Something",
                WordTranslation = "ўось",
                ImageSrc = "imgNotFound.png"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _defaultWordsService.AddDefaultWord(request1);
                _defaultWordsService.AddDefaultWord(request2);
            });
        }

        [Fact]
        public void AddDefaultWord_ProperDefaultWordDetails()
        {
            //Arrange
            DefaultWordAddRequest? request = new DefaultWordAddRequest()
            {
                Word = "Something",
                WordTranslation = "ўось",
                ImageSrc = "image.png"
            };

            //Act
            DefaultWordResponse response = _defaultWordsService.AddDefaultWord(request);
            List<DefaultWordResponse> defaultWords_from_GetAllDefaultWords = _defaultWordsService.GetAllDefaultWords();

            //Assert
            Assert.True(response.DefaultWordId != Guid.Empty);
            Assert.Contains(response, defaultWords_from_GetAllDefaultWords);
        }
        #endregion

        #region GetAllDefaultWords

        [Fact]
        public void GetAllDefaultWords_EmptyList()
        {
            //Act
            List<DefaultWordResponse> actual_defaultWords_response_list = _defaultWordsService.GetAllDefaultWords();

            //Assert
            Assert.Empty(actual_defaultWords_response_list);
        }

        [Fact]
        public void GetAllDefaultWords_AddFewDefaultWords()
        {
            //Arrange
            List<DefaultWordAddRequest> defaultWords_request_list = new List<DefaultWordAddRequest>()
            {
                new DefaultWordAddRequest() {Word = "Something", WordTranslation = "ўось", ImageSrc = "imageNotFound.png"},
                new DefaultWordAddRequest() {Word = "Someone", WordTranslation = "’тось", ImageSrc = "imageNotFound.png"}
            };

            //Act
            List<DefaultWordResponse> defaultWords_list_from_add_defaultWord = new List<DefaultWordResponse>();
            foreach (DefaultWordAddRequest defaultWord_request in defaultWords_request_list)
            {
                defaultWords_list_from_add_defaultWord.Add(_defaultWordsService.AddDefaultWord(defaultWord_request));
            }

            List<DefaultWordResponse> actualDefaultWordsResponseList = _defaultWordsService.GetAllDefaultWords();

            //Assert
            foreach (DefaultWordResponse expected_defaultWord in defaultWords_list_from_add_defaultWord)
            {
                Assert.Contains(expected_defaultWord, actualDefaultWordsResponseList);
            }
        }

        #endregion

        #region GetDefaultWordById

        [Fact]
        public void GetDefaultWordById_NullDefaultWordId()
        {
            //Arrange
            Guid? defaultWordId = null;
            
            //Act
            DefaultWordResponse? defaultWord_response_from_get_method = 
                _defaultWordsService.GetDefaultWordById(defaultWordId);

            //Assert
            Assert.Null(defaultWord_response_from_get_method);
        }

        [Fact]
        public void GetDefaultWordById_ValidDefaultWordId()
        {
            //Arrange
            DefaultWordAddRequest? defaultWord_add_request = new DefaultWordAddRequest()
            {
                Word = "Something",
                WordTranslation = "ўось",
                ImageSrc = "imgNotFound.png"
            };
            DefaultWordResponse defaultWord_response_from_add = _defaultWordsService.AddDefaultWord(defaultWord_add_request);

            //Act
            DefaultWordResponse? defaultWord_from_get = _defaultWordsService.GetDefaultWordById
                (defaultWord_response_from_add.DefaultWordId);

            //Assert
            Assert.Equal(defaultWord_response_from_add, defaultWord_from_get);
        }

        #endregion
    }
}