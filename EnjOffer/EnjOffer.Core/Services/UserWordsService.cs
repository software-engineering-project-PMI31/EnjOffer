﻿using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.DTO;
using EnjOffer.Core.Helpers;
using EnjOffer.Core.ServiceContracts;
using EnjOffer.Core.Domain.RepositoryContracts;

namespace EnjOffer.Core.Services
{
    public class UserWordsService : IUserWordsService
    {
        private readonly IUserWordsRepository _userWordsRepository;

        public UserWordsService(IUserWordsRepository userWordsRepository)
        {
            _userWordsRepository = userWordsRepository;
        }

        public double GetPriority(DateTime? lastTimeEntered, int correctlyEntered, int incorrectlyEntererd)
        {
            const double correctlyEnteredWeight = 0.9;
            const double incorrectlyEnteredWeight = 0.1;
            double decayRate = (lastTimeEntered is not null) &&
                (DateTime.Now - ((DateTime)lastTimeEntered)).TotalHours > 200 ? 600 : 400;

            return (lastTimeEntered is not null)
                ? ((double)correctlyEntered * correctlyEnteredWeight) / ((incorrectlyEntererd * incorrectlyEnteredWeight) +
                (correctlyEntered * correctlyEnteredWeight)) * (1 - Math.Exp(-(DateTime.Now - ((DateTime)lastTimeEntered)).TotalHours / decayRate))
                : 1;
        }

        public UserWordsResponse AddUserWord(UserWordsAddRequest? userWordAddRequest)
        {
            if (userWordAddRequest is null)
            {
                throw new ArgumentNullException(nameof(userWordAddRequest));
            }

            ValidationHelper.ModelValidation(userWordAddRequest);

            if (userWordAddRequest.Word is not null &&
                userWordAddRequest.WordTranslation is not null &&
                _userWordsRepository.GetUserWordByWordAndTranslation(userWordAddRequest.Word, userWordAddRequest.WordTranslation) is not null)
            {
                throw new ArgumentException("This word and translation already exist", nameof(userWordAddRequest));
            }

            //Convert userWordAddRequest to UserWords type
            UserWords? userWord = userWordAddRequest.ToUserWords();

            //Generate UserWordId
            userWord.UserWordId = Guid.NewGuid();

            //Generate Datetime and default correct and incorrect answers
            userWord.LastTimeEntered = null;
            userWord.CorrectEnteredCount = 0;
            userWord.IncorrectEnteredCount = 0;

            _userWordsRepository.AddUserWord(userWord);

            return userWord.ToUserWordsResponse(this);
        }

        public List<UserWordsResponse> GetAllUserWords()
        {
            return _userWordsRepository.GetAllUserWords().Select(userWord => userWord.ToUserWordsResponse(this)).ToList();
        }

        public UserWordsResponse? GetUserWordById(Guid? userWordId)
        {
            if (userWordId is null)
            {
                return null;
            }

            UserWords? userWord_response_from_list = _userWordsRepository.GetUserWordById(userWordId);

            return userWord_response_from_list?.ToUserWordsResponse(this) ?? null;
        }

        public bool DeleteUserWord(Guid? userWordId)
        {
            if (userWordId is null)
            {
                throw new ArgumentNullException(nameof(userWordId));
            }

            UserWords? userWord = _userWordsRepository.GetUserWordById(userWordId);

            if (userWord is null)
            {
                return false;
            }

            _userWordsRepository.DeleteUserWord(userWordId);

            return true;
        }

        public List<UserWordsResponse> GetUserWordsByDate(DateTime? dateTime)
        {
            return _userWordsRepository.GetUserWordsByDate(dateTime).Select(temp => temp.ToUserWordsResponse(this)).ToList();
        }

        public UserWordsResponse UpdateUserWord(UserWordsUpdateRequest? userWordsUpdateRequest)
        {
            if (userWordsUpdateRequest is null)
            {
                throw new ArgumentNullException(nameof(userWordsUpdateRequest));
            }

            ValidationHelper.ModelValidation(userWordsUpdateRequest);

            UserWords? matchingUserWord = _userWordsRepository.GetUserWordById(userWordsUpdateRequest.UserWordId);
            if (matchingUserWord is null)
            {
                throw new ArgumentException("Given user's word doesn't exist");
            }

            matchingUserWord.LastTimeEntered = userWordsUpdateRequest.LastTimeEntered ?? matchingUserWord.LastTimeEntered;
            matchingUserWord.CorrectEnteredCount = userWordsUpdateRequest.CorrectEnteredCount ?? matchingUserWord.CorrectEnteredCount;
            matchingUserWord.IncorrectEnteredCount = userWordsUpdateRequest.IncorrectEnteredCount ?? matchingUserWord.IncorrectEnteredCount;
            matchingUserWord.CorrectEnteredCount += userWordsUpdateRequest.IsIncreaseCorrectEnteredCount ? 1 : 0;
            matchingUserWord.IncorrectEnteredCount += userWordsUpdateRequest.IsIncreaseIncorrectEnteredCount ? 1 : 0;

            _userWordsRepository.UpdateUserWord(matchingUserWord);

            //_userWordsRepository
            return matchingUserWord.ToUserWordsResponse(this);
        }

        public List<UserWordsResponse> GetUserWordsSortedByPriority(List<UserWordsResponse> userWords)
        {
            return userWords.OrderByDescending(temp => temp.Priority).ToList();
        }

        public UserWordsResponse? GetUserWordToShow()
        {
            List<UserWordsResponse> userWords = 
                _userWordsRepository.GetAllUserWords().Select(userWord => userWord.ToUserWordsResponse(this)).ToList();

            return userWords.OrderByDescending(temp => temp.Priority).ElementAt(0);
        }
    }
}
