using Microsoft.Extensions.DependencyInjection;

namespace Gaspra.Logging.ApplicationInformation.Extensions
{
    public static class ApplicationInformationExtensions
    {
        public static IServiceCollection AddApplicationInformation(this IServiceCollection services)
        {
            services.AddSingleton<IApplicationInformation, ApplicationInformation>();

            return services;
        }
    }
}
