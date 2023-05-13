using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnjOffer.Core.DTO;

namespace EnjOffer.Core.ServiceContracts
{
    public interface IUserStatisticsService
    {
        UserStatisticsResponse AddUserStatistics(UserStatisticsAddRequest? userStatisticsAddRequest);
        List<UserStatisticsResponse> GetAllUserStatistics(Guid userId);
        UserStatisticsResponse? GetUserStatisticsByDate(DateTime? dateTime);
        UserStatisticsResponse? GetUserStatisticsByDateAndUserId(DateTime? dateTime, Guid? userId);
        UserStatisticsResponse? GetUserStatisticsById(Guid? userStatisticsId);
        UserStatisticsResponse UpdateUserStatistics(UserStatisticsUpdateRequest? userStatisticsUpdateRequest);
    }
}
