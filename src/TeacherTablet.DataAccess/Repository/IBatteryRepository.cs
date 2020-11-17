using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherTablet.DataAccess.Entities;

namespace TeacherTablet.DataAccess.Repository
{
    public interface IBatteryRepository
    {
        Task<IEnumerable<Battery>> GetBatteriesAsync();
    }
}
