
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeacherTablet.Business.Models;
using TeacherTablet.DataAccess.Entities;
using TeacherTablet.DataAccess.Repository;

namespace TeacherTablet.Business.Tests
{
    [TestClass]
    public class BatteryBusiness_Tests
    {
        private Mock<IBatteryRepository> _mockBatteryRepo;
        private Mock<ILogger<BatteryBusiness>> _mockLogger;
        private IBatteryBusiness _batteryBusiness;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockBatteryRepo = new Mock<IBatteryRepository>();
            _mockLogger = new Mock<ILogger<BatteryBusiness>>();
            _batteryBusiness = new BatteryBusiness(_mockBatteryRepo.Object, _mockLogger.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockBatteryRepo.VerifyAll();
            _mockLogger.VerifyAll();
        }

        // Success test case
        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_SingleBatteryNotCharged()
        {
            //arrange
            IEnumerable<Battery> batteries = GetBatteryDataPointsWithoutChargeInBetween();

            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .ReturnsAsync(batteries);

            //act
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNotNull(result);
            var average = result.FirstOrDefault();
            Assert.AreEqual(average.SerialNumber, "Device-1");
            Assert.AreEqual(average.AverageDailyBatteryUsage, "0.08");
            Assert.AreEqual(average.NeedsReplacement, false);
        }

        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_BatteryCharged()
        {
            //arrange
            IEnumerable<Battery> batteries = GetBatteryDataPointsWithChargeInBetween();

            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .ReturnsAsync(batteries);

            //act
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNotNull(result);
            var average = result.FirstOrDefault();
            Assert.AreEqual(average.SerialNumber, "Device-1");
            Assert.AreEqual(average.AverageDailyBatteryUsage, "0.08");
            Assert.AreEqual(average.NeedsReplacement, false);
        }

        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_DeviceNull()
        {
            //arrange
            IEnumerable<Battery> batteries = GetBatteryDataPointsWithNullDeviceNumber();

            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .ReturnsAsync(batteries);

            //act
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(batteries.GroupBy(x => x.SerialNumber).Count() - 1, result.Count());
            var average = result.FirstOrDefault(x => x.SerialNumber == "Device-1");
            Assert.AreEqual(average.SerialNumber, "Device-1");
            Assert.AreEqual(average.AverageDailyBatteryUsage, "0.08");
            Assert.AreEqual(average.NeedsReplacement, false);
        }

        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_NegativeBatteryLevel()
        {
            //arrange
            IEnumerable<Battery> batteries = GetBatteryDataPointsWithNegativeBatteryLevel();

            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .ReturnsAsync(batteries);

            //act
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(batteries.GroupBy(x => x.SerialNumber).Count(), result.Count());

            var averageDevice1 = result.FirstOrDefault(x => x.SerialNumber == "Device-1");
            Assert.AreEqual(averageDevice1.SerialNumber, "Device-1");
            Assert.AreEqual(averageDevice1.AverageDailyBatteryUsage, "0.08");
            Assert.AreEqual(averageDevice1.NeedsReplacement, false);

            var averageDevice2 = result.FirstOrDefault(x => x.SerialNumber == "Device-2");
            Assert.AreEqual(averageDevice2.SerialNumber, "Device-2");
            Assert.AreEqual(averageDevice2.AverageDailyBatteryUsage, "Unknown");
            Assert.AreEqual(averageDevice2.NeedsReplacement, false);
        }

        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_SingleDataPoint()
        {
            //arrange
            IEnumerable<Battery> batteries = GetBatteryDataPointsWithNegativeBatteryLevel();

            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .ReturnsAsync(batteries);

            //act
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(batteries.GroupBy(x => x.SerialNumber).Count(), result.Count());
            var averageDevice1 = result.FirstOrDefault(x => x.SerialNumber == "Device-2");
            Assert.AreEqual(averageDevice1.SerialNumber, "Device-2");
            Assert.AreEqual(averageDevice1.AverageDailyBatteryUsage, "Unknown");
            Assert.AreEqual(averageDevice1.NeedsReplacement, false);
        }

        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_InefficientBattery()
        {
            //arrange
            IEnumerable<Battery> batteries = GetBatteryDataPointsWithInefficientBattery();

            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .ReturnsAsync(batteries);

            //act
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(batteries.GroupBy(x => x.SerialNumber).Count(), result.Count());
            var averageDevice1 = result.FirstOrDefault(x => x.SerialNumber == "Device-1");
            Assert.AreEqual(averageDevice1.SerialNumber, "Device-1");
            Assert.AreEqual(averageDevice1.AverageDailyBatteryUsage, "0.38");
            Assert.AreEqual(averageDevice1.NeedsReplacement, true);
        }

        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_NoData()
        {
            //arrange
            IEnumerable<Battery> batteries = null;

            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .ReturnsAsync(batteries);

            //act
            var result = await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_Exception_DataSourceFailure()
        {
            //arrange
            string errorMessage = "Exception encountered";
            _mockBatteryRepo
                .Setup(x => x.GetBatteriesAsync())
                .Throws(new Exception(errorMessage));

            //act & assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(async () => await _batteryBusiness
                                    .GetAverageDailyBatteryUsageAsync()
                                    .ConfigureAwait(false));

            //assert
            Assert.AreEqual(exception.Message, errorMessage);
        }

        #region private methods
        private IEnumerable<Battery> GetBatteryDataPointsWithoutChargeInBetween()
        {
            return new List<Battery>()
            {
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,10,15,12,30),
                    BatteryLevel = 0.98M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,11,15,12,30),
                    BatteryLevel = 0.90M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,12,15,12,30),
                    BatteryLevel = 0.82M,
                    SerialNumber = "Device-1"
                }
            };
        }

        private IEnumerable<Battery> GetBatteryDataPointsWithChargeInBetween()
        {
            return new List<Battery>()
            {
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,10,15,12,30),
                    BatteryLevel = 0.98M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,11,15,12,30),
                    BatteryLevel = 0.90M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,12,15,12,30),
                    BatteryLevel = 0.99M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,13,15,12,30),
                    BatteryLevel = 0.91M,
                    SerialNumber = "Device-1"
                }
            };
        }

        private IEnumerable<Battery> GetBatteryDataPointsWithNullDeviceNumber()
        {
            return new List<Battery>()
            {
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,10,15,12,30),
                    BatteryLevel = 0.98M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,11,15,12,30),
                    BatteryLevel = 0.90M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,11,15,12,30),
                    BatteryLevel = 0.90M,
                    SerialNumber = null
                }
            };
        }

        private IEnumerable<Battery> GetBatteryDataPointsWithNegativeBatteryLevel()
        {
            return new List<Battery>()
            {
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,10,15,12,30),
                    BatteryLevel = 0.98M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,11,15,12,30),
                    BatteryLevel = 0.90M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,11,15,12,30),
                    BatteryLevel = -0.90M,
                    SerialNumber = "Device-2"
                }
            };
        }

        private IEnumerable<Battery> GetBatteryDataPointsWithSingleDataPointBatteryLevel()
        {
            return new List<Battery>()
            {
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,10,15,12,30),
                    BatteryLevel = 0.98M,
                    SerialNumber = "Device-1"
                }
            };
        }

        private IEnumerable<Battery> GetBatteryDataPointsWithInefficientBattery()
        {
            return new List<Battery>()
            {
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,10,15,12,30),
                    BatteryLevel = 0.98M,
                    SerialNumber = "Device-1"
                },
                new Battery
                {
                    TimeStamp = new DateTime(2020,10,11,15,12,30),
                    BatteryLevel = 0.60M,
                    SerialNumber = "Device-1"
                }
            };
        }
        #endregion
    }
}
