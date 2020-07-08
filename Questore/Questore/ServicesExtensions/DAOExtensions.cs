using Microsoft.Extensions.DependencyInjection;
using Questore.Persistence;
using Questore.Photos;

namespace Questore.ServicesExtensions
{
    public static class DAOExtensions
    {
        public static IServiceCollection AddDataProviders(this IServiceCollection services)
        {
            services.AddScoped<IArtifactDAO, ArtifactDAO>()
                .AddScoped<IQuestDAO, QuestDAO>()
                .AddScoped<IStudentDAO, StudentDAO>()
                .AddScoped<IDetailsDAO, DetailsDAO>()
                .AddScoped<IPhotoAccessor, PhotoAccessor>();

            return services;
        }
    }
}
