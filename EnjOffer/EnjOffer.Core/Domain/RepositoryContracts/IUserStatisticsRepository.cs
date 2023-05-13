using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.Domain.Entities;

namespace EnjOffer.Core.Domain.RepositoryContracts
{
    public interface IUserStatisticsRepository
    {
        UserStatistics AddUserStatisticRecord(UserStatistics userStatisticsRecord);
        List<UserStatistics> GetAllUserStatistics();
        UserStatistics? GetUserStatisticsByDate(DateTime? dateTime);
        UserStatistics? GetStatisticsById(Guid? userStatisticsId);
        UserStatistics? GetStatisticsByDateAndUserId(DateTime? dateTime, Guid? userId);
        UserStatistics? GetStatisticsByIdAndUserId(Guid? userStatisticsId, Guid? userId);
        UserStatistics UpdateUserStatistics(UserStatistics userStatisticsUpdate);
    }
}
