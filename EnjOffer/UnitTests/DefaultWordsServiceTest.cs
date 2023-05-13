using Xunit;
using EnjOffer.Core.DTO;
using EnjOffer.Core.ServiceContracts;
using System;
using EnjOffer.Core.Services;
using System.Collections.Generic;
using AutoFixture;
using EnjOffer.Core.Domain.RepositoryContracts;
using Moq;
using EnjOffer.Core.Domain.Entities;
using System.Linq;
using EnjOffer.Core.Domain.IdentityEntities;

namespace UnitTests
{
    public class DefaultWordsServiceTest
    {
        /*private readonly IDefaultWordsService _defaultWordsService;*/
        private readonly Mock<IDefaultWordsRepository> _defaultWordsRepositoryMock;
        private readonly IDefaultWordsRepository _defaultWordsRepository;
        private readonly IDefaultWordsService _defaultWordsService;

        private readonly IFixture _fixture;

        //TO-DO: Avoid the Service Locator Anti-Pattern here. Use Dependency Injection
        public DefaultWordsServiceTest()
        {
            /*_defaultWordsService = new DefaultWordsService();*/
            _fixture = new Fixture();

            _defaultWordsRepositoryMock = new Mock<IDefaultWordsRepository>();
            _defaultWordsRepository = _defaultWordsRepositoryMock.Object;

            _defaultWordsService = new DefaultWordsService(_defaultWordsRepository);
        }

        #region AddDefaultWord
        [Fact]
        public void AddDefaultWord_NullDefaultWordDetails()
        {
            //Arrange
            DefaultWordAddRequest? request = null;
            DefaultWords defaultWord = _fixture.Build<DefaultWords>()
                .With(temp => temp.Users, null as ICollection<ApplicationUser>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .Create();

            _defaultWordsRepositoryMock.Setup(temp => temp.AddDefaultWord(It.IsAny<DefaultWords>())).Returns(defaultWord);

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

            DefaultWords defaultWord = _fixture.Build<DefaultWords>()
                .With(temp => temp.Users, null as ICollection<ApplicationUser>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .Create();

            _defaultWordsRepositoryMock.Setup(temp => temp.AddDefaultWord(It.IsAny<DefaultWords>())).Returns(defaultWord);

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

            DefaultWords defaultWord = _fixture.Build<DefaultWords>()
                .With(temp => temp.Users, null as ICollection<ApplicationUser>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .Create();

            _defaultWordsRepositoryMock.Setup(temp => temp.AddDefaultWord(It.IsAny<DefaultWords>())).Returns(defaultWord);

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

            DefaultWords first_defaultWord = request1.ToDefaultWords();

            _defaultWordsRepositoryMock.SetupSequence(temp => temp.AddDefaultWord(It.IsAny<DefaultWords>()))
                .Throws(new ArgumentException("The defaultWord with such word and translation already exist"))
                .Returns(first_defaultWord);

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Act
                _defaultWordsService.AddDefaultWord(request2);
            });
        }

        [Fact]
        public void AddDefaultWord_ProperDefaultWordDetails()
        {
            //Arrange
            DefaultWordAddRequest? request = _fixture.Create<DefaultWordAddRequest>();
            DefaultWords defaultWord = request.ToDefaultWords();
            DefaultWordResponse defaultWord_response = defaultWord.ToDefaultWordResponse();

            _defaultWordsRepositoryMock.Setup(temp => temp.AddDefaultWord(It.IsAny<DefaultWords>()))
                .Returns(defaultWord);

            //Act
            DefaultWordResponse defaultWords_response_from_add = _defaultWordsService.AddDefaultWord(request);
            
            defaultWord.DefaultWordId = defaultWords_response_from_add.DefaultWordId;
            defaultWord_response.DefaultWordId = defaultWords_response_from_add.DefaultWordId;

            //Assert
            Assert.True(defaultWords_response_from_add.DefaultWordId != Guid.Empty);
            Assert.Equal(defaultWords_response_from_add, defaultWord_response);
        }

        #endregion

        #region GetAllDefaultWords

        [Fact]
        public void GetAllDefaultWords_EmptyList()
        {
            //Arrange
            List<DefaultWords> defaultWords = new List<DefaultWords>();
            _defaultWordsRepositoryMock.Setup(temp => temp.GetAllDefaultWords()).Returns(defaultWords);

            //Act
            List<DefaultWordResponse> actual_defaultWords_response_list = _defaultWordsService.GetAllDefaultWords();

            //Assert
            Assert.Empty(actual_defaultWords_response_list);
        }

        [Fact]
        public void GetAllDefaultWords_AddFewDefaultWords()
        {
            //Arrange
            List<DefaultWords> defaultWords_list = new List<DefaultWords>()
            {
                _fixture.Build<DefaultWords>()
                .With(temp => temp.Users, null as ICollection<ApplicationUser>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .Create(),

                _fixture.Build<DefaultWords>()
                .With(temp => temp.Users, null as ICollection<ApplicationUser>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .Create()
            };

            List<DefaultWordResponse> defaultWords_response_list = defaultWords_list
                .Select(temp => temp.ToDefaultWordResponse()).ToList();

            _defaultWordsRepositoryMock.Setup(temp => temp.GetAllDefaultWords()).Returns(defaultWords_list);

            //Act
            List<DefaultWordResponse> actual_defaultWords_response_list = _defaultWordsService.GetAllDefaultWords();

            //Assert
            Assert.Equal(defaultWords_response_list, actual_defaultWords_response_list);
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
            DefaultWords defaultWord = _fixture.Build<DefaultWords>()
                .With(temp => temp.Users, null as ICollection<ApplicationUser>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .Create();

            DefaultWordResponse defaultWord_response_expected = defaultWord.ToDefaultWordResponse();
            _defaultWordsRepositoryMock.Setup(temp => temp.GetDefaultWordById(It.IsAny<Guid>())).Returns(defaultWord);

            //Act
            DefaultWordResponse? defaultWord_from_get = _defaultWordsService.GetDefaultWordById
                (defaultWord.DefaultWordId);

            //Assert
            Assert.Equal(defaultWord_response_expected, defaultWord_from_get);
        }

        #endregion

        #region DeleteDefaultWord

        [Fact]
        public void DeleteDefaultWord_ValidId()
        {
            //Arrange
            DefaultWords defaultWord = _fixture.Build<DefaultWords>()
                .With(temp => temp.Users, null as ICollection<ApplicationUser>)
                .With(temp => temp.UsersDefaultWords, null as ICollection<UsersDefaultWords>)
                .Create();

            _defaultWordsRepositoryMock.Setup(temp => temp.DeleteDefaultWord(It.IsAny<Guid>())).Returns(true);
            _defaultWordsRepositoryMock.Setup(temp => temp.GetDefaultWordById(It.IsAny<Guid>())).Returns(defaultWord);

            //Act
            bool isDeleted = _defaultWordsService.DeleteDefaultWord(defaultWord.DefaultWordId);

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