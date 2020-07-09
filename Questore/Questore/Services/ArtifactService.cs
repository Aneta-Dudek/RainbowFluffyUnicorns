using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Questore.Services
{
    public class ArtifactService : IArtifactService
    {
        private readonly ISession _session;
        private readonly IArtifactDAO _artifactDao;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));

        public ArtifactService(IServiceProvider services, IArtifactDAO artifactDao)
        {
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            _artifactDao = artifactDao;
        }

        public IEnumerable<Artifact> GetAffordableArtifacts()
        {
            var artifacts = _artifactDao.GetArtifacts().ToList();
            CheckAffordability(artifacts);
            return artifacts;
        }

        public void BuyArtifact(int id)
        {
            _artifactDao.BuyArtifact(id, ActiveStudent.Id);
        }

        public void UseArtifact(int id)
        {
            _artifactDao.UseArtifact(id);
        }

        public void DeleteStudentArtifact(int id)
        {
            _artifactDao.DeleteStudentArtifact(id);
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