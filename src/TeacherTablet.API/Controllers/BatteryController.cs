using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherTablet.Business;
using TeacherTablet.Business.Models;

namespace TeacherTablet.API.Controllers
{

    [Route("[controller]")]
    public class BatteryController : ControllerBase
    {
        private IBatteryBusiness _batteryBusiness;

        public BatteryController(IBatteryBusiness batteryBusiness)
        {
            _batteryBusiness = batteryBusiness;
        }

        /// <summary>
        /// Gets the average daily battery usage of the teacher tablet device
        /// </summary>
        /// <returns>IEnumerable&lt;BatteryUsage&gt;</returns>
        [HttpGet("AverageDailyBatteryUsage")]
        [Produces("application/json")]
        [SwaggerResponse(200, null, typeof(IEnumerable<BatteryUsage>))]
        public async Task<ActionResult<IEnumerable<BatteryUsage>>> GetAverageDailyBatteryUsageAsync()
        {
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            return Ok(result ?? new List<BatteryUsage>());
        }
    }
}
