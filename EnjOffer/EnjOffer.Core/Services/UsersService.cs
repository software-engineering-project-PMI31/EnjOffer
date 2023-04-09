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
        private readonly List<Users> _users;
        private readonly List<DefaultWords> _defaultWords;
        private readonly List<UsersDefaultWords> _usersDefaultWords;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            _users = new List<Users>();
            _defaultWords = new List<DefaultWords>();
            _usersDefaultWords = new List<UsersDefaultWords>();
        }

        public UserResponse AddUser(UserAddRequest? userAddRequest)
        {
            if (userAddRequest is null)
            {
                throw new ArgumentNullException(nameof(userAddRequest));
            }

            ValidationHelper.ModelValidation(userAddRequest);

            if (_users.Any(temp => temp.Email == userAddRequest.Email && temp.Password == userAddRequest.Password &&
            temp.Role == userAddRequest.Role))
            {
                throw new ArgumentException("This user already exists", nameof(userAddRequest));
            }

            //Convert userAddRequest to Users type
            Users user = userAddRequest.ToUser();

            //Generate UserId
            user.UserId = Guid.NewGuid();

            //Add user to list
            _users.Add(user);

            foreach (DefaultWords defaultWord in _defaultWords)
            {
                UsersDefaultWords userDefaultWord = new UsersDefaultWords()
                {
                    UserId = user.UserId,
                    DefaultWordId = defaultWord.DefaultWordId,
                    LastTimeEntered = null,
                    CorrectEnteredCount = 0,
                    IncorrectEnteredCount = 0

                };

                _usersDefaultWords.Add(userDefaultWord);
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

            Users? user = _users.FirstOrDefault(temp => temp.UserId == userId);

            if (user is null)
            {
                return false;
            }

            _users.RemoveAll(temp => temp.UserId == userId);

            return true;
        }

        public List<UserResponse> GetAllUsers()
        {
            return _users.Select(user => user.ToUserResponse()).ToList();
        }

        public UserResponse? GetUserById(Guid? userId)
        {
            if (userId is null)
            {
                return null;
            }

            Users? user_response_from_list = _users.FirstOrDefault
                (temp => temp.UserId == userId);

            return user_response_from_list?.ToUserResponse() ?? null;
        }
    }
}
