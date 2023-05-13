using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.RepositoryContracts;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Infrastructure.Repositories
{
    public class UserStatisticsRepository : IUserStatisticsRepository
    {
        private readonly EnjOfferDbContext _db;

        public UserStatisticsRepository(EnjOfferDbContext db)
        {
            _db = db;
        }

        public UserStatistics AddUserStatisticRecord(UserStatistics userStatisticsRecord)
        {
            _db.UserStatistics.Add(userStatisticsRecord);
            _db.SaveChanges();

            return userStatisticsRecord;
        }

        public List<UserStatistics> GetAllUserStatistics()
        {
            return _db.UserStatistics.ToList();
        }

        public UserStatistics? GetStatisticsById(Guid? userStatisticsId)
        {
            return _db.UserStatistics.FirstOrDefault(temp => temp.UserStatisticsId == userStatisticsId);
        }

        public UserStatistics? GetStatisticsByDateAndUserId(DateTime? dateTime, Guid? userId)
        {
            return _db.UserStatistics.FirstOrDefault(temp => temp.AnswerDate == dateTime &&
                temp.UserId == userId);
        }

        public UserStatistics? GetStatisticsByIdAndUserId(Guid? userStatisticsId, Guid? userId)
        {
            return _db.UserStatistics.FirstOrDefault(temp => temp.UserStatisticsId == userStatisticsId &&
                temp.UserId == userId);
        }

        public UserStatistics? GetUserStatisticsByDate(DateTime? dateTime)
        {
            return _db.UserStatistics.FirstOrDefault(temp => temp.AnswerDate == dateTime);
        }

        public UserStatistics UpdateUserStatistics(UserStatistics userStatisticsUpdate)
        {
            UserStatistics? matchingUserStatistics =
                _db.UserStatistics.FirstOrDefault(temp => temp.UserStatisticsId == userStatisticsUpdate.UserStatisticsId);

            if (matchingUserStatistics == null)
            {
                return userStatisticsUpdate;
            }

            matchingUserStatistics.CorrectAnswersCount = userStatisticsUpdate.CorrectAnswersCount;
            matchingUserStatistics.IncorrectAnswersCount = userStatisticsUpdate.IncorrectAnswersCount;

            int countUpdated = _db.SaveChanges();

            return matchingUserStatistics;
        }
    }
}
