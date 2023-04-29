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
        List<UserStatisticsResponse> GetAllUserStatistics();
        UserStatisticsResponse? GetUserStatisticsByDate(DateTime? dateTime);
        UserStatisticsResponse? GetUserStatisticsById(Guid? userStatisticsId);
        UserStatisticsResponse UpdateUserStatistics(UserStatisticsUpdateRequest? userStatisticsUpdateRequest);
    }
}
