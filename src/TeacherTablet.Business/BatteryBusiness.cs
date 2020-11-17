using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeacherTablet.Business.Models;
using TeacherTablet.DataAccess.Entities;
using TeacherTablet.DataAccess.Repository;

namespace TeacherTablet.Business
{
    public class BatteryBusiness : IBatteryBusiness
    {
        private readonly IBatteryRepository _batteryRepository;
        private readonly ILogger<BatteryBusiness> _logger;

        public BatteryBusiness(IBatteryRepository batteryRepository, ILogger<BatteryBusiness> logger)
        {
            _batteryRepository = batteryRepository;
            _logger = logger;
        }

        public async virtual Task<IEnumerable<BatteryUsage>> GetAverageDailyBatteryUsageAsync()
        {
            IEnumerable<Battery> batteries = await _batteryRepository
                                                        .GetBatteriesAsync()
                                                        .ConfigureAwait(false);

            if (batteries == null)
                return null;

            ConcurrentBag<BatteryUsage> batteriesUsage = new ConcurrentBag<BatteryUsage>();

            Parallel.ForEach(batteries.GroupBy(x => x.SerialNumber), batteryDataPoints =>
            {
                if (string.IsNullOrWhiteSpace(batteryDataPoints.Key))
                    return;

                decimal usageLevel = 0.00M;

                try
                {
                    decimal previousDataPointUsageLevel = 0.00M;
                    DateTime previousDataPointTimeStamp = new DateTime();
                    int totalSeconds = 0;
                    decimal totalDischarge = 0.00M;

                    foreach (var batteryDataPoint in batteryDataPoints.OrderBy(x => x.TimeStamp))
                    {
                        if (batteryDataPoint.BatteryLevel > 1.00M || batteryDataPoint.BatteryLevel < 0.00M)
                        {
                            string errorMessage = $"Invalid BatteryLevel SerialNumber:{batteryDataPoint.SerialNumber}, BatteryLevel:{batteryDataPoint.BatteryLevel}";
                            throw new NotSupportedException(errorMessage);
                        }

                        if (previousDataPointUsageLevel >= batteryDataPoint.BatteryLevel)
                        {
                            totalDischarge += (previousDataPointUsageLevel - batteryDataPoint.BatteryLevel);
                            totalSeconds += (int)(batteryDataPoint.TimeStamp - previousDataPointTimeStamp).TotalSeconds;
                        }

                        previousDataPointUsageLevel = batteryDataPoint.BatteryLevel;
                        previousDataPointTimeStamp = batteryDataPoint.TimeStamp;
                    }

                    usageLevel = (totalDischarge * 86400) / (totalSeconds == 0 ? 1 : totalSeconds);
                    usageLevel = decimal.Round(usageLevel, 2, MidpointRounding.AwayFromZero);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occured while processing for the Battery:{batteryDataPoints.Key}");
                }
                finally
                {
                    batteriesUsage.Add(new BatteryUsage
                    {
                        SerialNumber = batteryDataPoints.Key,
                        AverageDailyBatteryUsage = usageLevel != 0.00M ? usageLevel.ToString() : "Unknown",
                        NeedsReplacement = usageLevel > 0.30M
                    }); ;
                }
            });

            return batteriesUsage;
        }
    }
}
