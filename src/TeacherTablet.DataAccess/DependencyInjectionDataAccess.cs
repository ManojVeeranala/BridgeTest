using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TeacherTablet.DataAccess.Repository;

namespace TeacherTablet.DataAccess
{
    public static class DependencyInjectionDataAccess
    {
        public static IServiceCollection RegisterDataAccessLayer(this IServiceCollection services)
        {
            services.AddSingleton<IBatteryRepository, BatteryRepository>();

            return services;
        }
    }
}
