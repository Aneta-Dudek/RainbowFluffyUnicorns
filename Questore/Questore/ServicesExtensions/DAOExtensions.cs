using Microsoft.Extensions.DependencyInjection;
using Questore.Data.Interfaces;
using Questore.Data.Persistence;
using Questore.Data.Photos;
using Questore.Services;

namespace Questore.ServicesExtensions
{
    public static class DAOExtensions
    {
        public static IServiceCollection AddDataProviders(this IServiceCollection services)
        {
            services.AddScoped<IArtifactDAO, ArtifactDAO>()
                .AddScoped<IArtifactService, ArtifactService>()
                .AddScoped<IQuestDAO, QuestDAO>()
                .AddScoped<IQuestService, QuestService>()
                .AddScoped<IStudentDAO, StudentDAO>()
                .AddScoped<IDetailsDAO, DetailsDAO>()
                .AddScoped<IPhotoAccessor, PhotoAccessor>();

            return services;
        }
    }
}
