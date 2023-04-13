using EnjOffer.Core.Domain.Entities;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.DTO;
using EnjOffer.Core.Helpers;
using EnjOffer.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnjOffer.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IDefaultWordsRepository _defaultWordsRepository;
        private readonly IUsersDefaultWordsRepository _usersDefaultWordsRepository;

        public UsersService(IUsersRepository usersRepository, IDefaultWordsRepository defaultWordsRepository,
            IUsersDefaultWordsRepository usersDefaultWordsRepository)
        {
            _usersRepository = usersRepository;
            _defaultWordsRepository = defaultWordsRepository;
            _usersDefaultWordsRepository = usersDefaultWordsRepository;

        }

        public UserResponse AddUser(UserAddRequest? userAddRequest)
        {
            if (userAddRequest is null)
            {
                throw new ArgumentNullException(nameof(userAddRequest));
            }

            ValidationHelper.ModelValidation(userAddRequest);

            if (_usersRepository.GetAllUsers().Any(temp => temp.Email == userAddRequest.Email && temp.Password == userAddRequest.Password &&
            temp.Role == userAddRequest.Role))
            {
                throw new ArgumentException("This user already exists", nameof(userAddRequest));
            }

            //Convert userAddRequest to Users type
            Users user = userAddRequest.ToUser();

            //Generate UserId
            user.UserId = Guid.NewGuid();

            //Add user to list
            _usersRepository.AddUser(user);

            foreach (DefaultWords defaultWord in _defaultWordsRepository.GetAllDefaultWords())
            {
                UsersDefaultWords userDefaultWord = new UsersDefaultWords()
                {
                    UserId = user.UserId,
                    DefaultWordId = defaultWord.DefaultWordId,
                    LastTimeEntered = null,
                    CorrectEnteredCount = 0,
                    IncorrectEnteredCount = 0

                };

                _usersDefaultWordsRepository.AddUserDefaultWord(userDefaultWord);
            }

            //Convert the Users object into UserResponse type
            return user.ToUserResponse();
        }

        public bool DeleteUser(Guid? userId)
        {
            if (userId is null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Users? user = _usersRepository.GetAllUsers().FirstOrDefault(temp => temp.UserId == userId);

            if (user is null)
            {
                return false;
            }

            _usersRepository.GetAllUsers().RemoveAll(temp => temp.UserId == userId);

            return true;
        }

        public List<UserResponse> GetAllUsers()
        {
            return _usersRepository.GetAllUsers().Select(user => user.ToUserResponse()).ToList();
        }

        public UserResponse? GetUserById(Guid? userId)
        {
            if (userId is null)
            {
                return null;
            }

            Users? user_response_from_list = _usersRepository.GetAllUsers().FirstOrDefault
                (temp => temp.UserId == userId);

            return user_response_from_list?.ToUserResponse() ?? null;
        }
    }
}
