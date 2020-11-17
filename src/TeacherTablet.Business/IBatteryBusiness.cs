using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherTablet.Business.Models;

namespace TeacherTablet.Business
{
    public interface IBatteryBusiness
    {
        Task<IEnumerable<BatteryUsage>> GetAverageDailyBatteryUsageAsync();
    }
}
