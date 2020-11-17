using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeacherTablet.API.Controllers;
using TeacherTablet.Business;
using TeacherTablet.Business.Models;

namespace TeacherTablet.API.Tests
{
    [TestClass]
    public class BatteryController_Tests
    {
        private Mock<IBatteryBusiness> _mockBatteryBusiness;
        private BatteryController _batteryController;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockBatteryBusiness = new Mock<IBatteryBusiness>();
            _batteryController = new BatteryController(_mockBatteryBusiness.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _mockBatteryBusiness.VerifyAll();
        }

        // Success test case
        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsSuccessResult_EnumerableOfBatteryUsage()
        {
            //arrange
            string serialNumber = "GFSHBWD-JGHBEDJHS";
            string batteryLevel = "0.02";

            IEnumerable<BatteryUsage> batteriesUsage = new List<BatteryUsage>()
            {
                new BatteryUsage{ SerialNumber = serialNumber, AverageDailyBatteryUsage = batteryLevel}
            };

            _mockBatteryBusiness
                .Setup(x => x.GetAverageDailyBatteryUsageAsync())
                .ReturnsAsync(batteriesUsage);

            //act
            var response = await _batteryController.GetAverageDailyBatteryUsageAsync().ConfigureAwait(false);

            //assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType((response.Result as OkObjectResult)?.Value, typeof(IEnumerable<BatteryUsage>));
            var records = (response.Result as OkObjectResult)?.Value as IEnumerable<BatteryUsage>;
            Assert.AreEqual(1, records?.Count());
            Assert.AreEqual(batteriesUsage, records);
        }

        //When the inner layers throws exception
        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsException_WhenExceptionInBatteryBusiness()
        {
            //arrange
            string errorMessage = "Invalid Data Source.";
            _mockBatteryBusiness
                .Setup(x => x.GetAverageDailyBatteryUsageAsync())
                .Throws(new Exception(errorMessage));

            //act & assert
            var exception = await Assert.ThrowsExceptionAsync<Exception>(async () => await _batteryController
                                        .GetAverageDailyBatteryUsageAsync()
                                        .ConfigureAwait(false));

            //assert
            Assert.AreEqual(exception.Message, errorMessage);
        }

        //When the Batteries data is null
        [TestMethod]
        public async Task GetAverageDailyBatteryUsageAsync_ReturnsZeroResult_WhenBatteryUsageDataNull()
        {
            //arrange
            IEnumerable<BatteryUsage> batteriesUsage = null;
            _mockBatteryBusiness
                .Setup(x => x.GetAverageDailyBatteryUsageAsync())
                .ReturnsAsync(batteriesUsage);

            //act
            var response = await _batteryController.GetAverageDailyBatteryUsageAsync().ConfigureAwait(false);

            //assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType((response.Result as OkObjectResult)?.Value, typeof(IEnumerable<BatteryUsage>));
            var records = (response.Result as OkObjectResult)?.Value as IEnumerable<BatteryUsage>;
            Assert.AreEqual(0, records.Count());
        }
    }
}
