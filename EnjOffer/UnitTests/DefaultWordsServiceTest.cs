using Xunit;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using System;
using EnjOffer.Core.Services;
using System.Collections.Generic;
using AutoFixture;

namespace UnitTests
{
    public class DefaultWordsServiceTest
    {
        private readonly IDefaultWordsService _defaultWordsService;
        private readonly IFixture _fixture;

        //TO-DO: Avoid the Service Locator Anti-Pattern here. Use Dependency Injection
        public DefaultWordsServiceTest()
        {
            _defaultWordsService = new DefaultWordsService();
            _fixture = new Fixture();
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
            DefaultWordAddRequest? request = _fixture.
                Build<DefaultWordAddRequest>()
                .With(temp => temp.Word, null as string).Create();

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
            DefaultWordAddRequest? request = _fixture.
                Build<DefaultWordAddRequest>()
                .With(temp => temp.WordTranslation, null as string).Create();

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
                WordTranslation = "����",
                ImageSrc = "imgNotFound.png"
            };
            DefaultWordAddRequest? request2 = new DefaultWordAddRequest()
            {
                Word = "Something",
                WordTranslation = "����",
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
            DefaultWordAddRequest? request = _fixture.Create<DefaultWordAddRequest>();

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
                _fixture.Create<DefaultWordAddRequest>(),
                _fixture.Create<DefaultWordAddRequest>()
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
            DefaultWordAddRequest? defaultWord_add_request = _fixture.Create<DefaultWordAddRequest>();
            DefaultWordResponse defaultWord_response_from_add = _defaultWordsService.AddDefaultWord(defaultWord_add_request);

            //Act
            DefaultWordResponse? defaultWord_from_get = _defaultWordsService.GetDefaultWordById
                (defaultWord_response_from_add.DefaultWordId);

            //Assert
            Assert.Equal(defaultWord_response_from_add, defaultWord_from_get);
        }

        #endregion

        #region DeleteDefaultWord

        [Fact]
        public void DeleteDefaultWord_ValidId()
        {
            //Arrange
            DefaultWordAddRequest defaultWord_add_request = _fixture.Create<DefaultWordAddRequest>();
            DefaultWordResponse defaultWord_response_from_add = _defaultWordsService.AddDefaultWord(defaultWord_add_request);

            //Act
            bool isDeleted = _defaultWordsService.DeleteDefaultWord(defaultWord_response_from_add.DefaultWordId);

            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeleteDefaultWord_InvalidId()
        {
            //Act
            bool isDeleted = _defaultWordsService.DeleteDefaultWord(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }

        #endregion
    }
}