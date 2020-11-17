using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TeacherTablet.Common;
using TeacherTablet.DataAccess.Entities;

namespace TeacherTablet.DataAccess.Repository
{
    public class BatteryRepository : IBatteryRepository
    {
        private readonly IOptions<Settings> _options;

        public BatteryRepository(IOptions<Settings> options)
        {
            _options = options;
        }

        public async virtual Task<IEnumerable<Battery>> GetBatteriesAsync()
        {
            var content = await File.ReadAllTextAsync(_options.Value.JsonDataSourcePath);
            return JsonConvert.DeserializeObject<IEnumerable<Battery>>(content);
        }
    }
}
