using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Questore.Services
{
    public class ArtifactService : ArtifactDAO, IArtifactService
    {
        private readonly ISession _session;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));

        public ArtifactService(IServiceProvider services, IConfiguration configuration) : base(configuration)
        {
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }

        public IEnumerable<Artifact> GetAffordableArtifacts()
        {
            var artifacts = GetArtifacts().ToList();
            CheckAffordability(artifacts);
            return artifacts;
        }

        public void BuyArtifact(int id)
        {
            BuyArtifact(id, ActiveStudent.Id);
        }

        private void CheckAffordability(IEnumerable<Artifact> artifacts)
        {
            foreach (var artifact in artifacts)
            {
                artifact.IsAffordable = ActiveStudent.Coolcoins >= artifact.Price;
            }
        }
    }
}