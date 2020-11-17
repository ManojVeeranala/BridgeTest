using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using TeacherTablet.Common;
using TeacherTablet.DataAccess.Repository;

namespace TeacherTablet.DataAccess.Tests
{
    [TestClass]
    public class BatteryRepository_Tests
    {

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        public async Task GetBatteriesAsync_SuccessResult_ValidOptions()
        {
            //arrange
            var settings = Options.Create(new Settings() { JsonDataSourcePath= "battery.json"});
            var batteryRepository = new BatteryRepository(settings);

            //act
            var result = await batteryRepository.GetBatteriesAsync()
                                    .ConfigureAwait(false);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }


        [TestMethod]
        public async Task GetBatteriesAsync_Exception_InvalidPath()
        {
            //arrange
            var settings = Options.Create(new Settings() { JsonDataSourcePath = "" });
            var batteryRepository = new BatteryRepository(settings);

            //act & assert
            var result = await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>  await batteryRepository.GetBatteriesAsync()
                                                                                                    .ConfigureAwait(false));
        }
    }
}
