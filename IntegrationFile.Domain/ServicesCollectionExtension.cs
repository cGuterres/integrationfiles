using IntegrationFile.Domain.Services.Contracts;
using IntegrationFile.Domain.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationFiles.Domain
{
    public static class ServicesCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IProcessFileService, ProcessFileService>();
        }
    }
}
