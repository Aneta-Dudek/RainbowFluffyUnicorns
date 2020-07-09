using Microsoft.Extensions.DependencyInjection;
using Questore.Data.Interfaces;
using Questore.Data.Persistence;
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
                .AddScoped<IAdminDAO, AdminDAO>()
                .AddScoped<IPhotoAccessor, PhotoAccessor>();

            return services;
        }
    }
}
