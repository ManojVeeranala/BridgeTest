using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using TeacherTablet.DataAccess;

namespace TeacherTablet.Business
{
    public static class DependencyInjectionBusiness
    {
        public static IServiceCollection RegisterBusinessLayer(this IServiceCollection services)
        {
            services.AddScoped<IBatteryBusiness, BatteryBusiness>();

            services.RegisterDataAccessLayer();

            return services;
        }
    }
}
