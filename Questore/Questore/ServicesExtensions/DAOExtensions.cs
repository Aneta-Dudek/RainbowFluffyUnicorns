using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Questore.Persistence;

namespace Questore.ServicesExtensions
{
    public static class DAOExtensions
    {
        public static IServiceCollection AddDataProviders(this IServiceCollection services)
        {
            services.AddScoped<IArtifactDAO, ArtifactDAO>()
                .AddScoped<IQuestDAO, QuestDAO>()
                .AddScoped<IStudentDAO, StudentDAO>();

            return services;
        }
    }
}
