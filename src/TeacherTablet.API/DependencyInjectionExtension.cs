using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeacherTablet.Business;

namespace TeacherTablet.API
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterApiLayer(this IServiceCollection services)
        {
            services.RegisterBusinessLayer();
            return services;
        }
    }
}
