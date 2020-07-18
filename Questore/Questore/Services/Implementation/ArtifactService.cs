using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Questore.Data.Interfaces;
using Questore.Data.Models;
using Questore.Services.Interfaces;

namespace Questore.Services.Implementation
{
    public class ArtifactService : IArtifactService
    {
        private readonly ISession _session;
        private readonly IArtifactDAO _artifactDao;

        private User ActiveUser => JsonSerializer.Deserialize<User>(_session.GetString("user"));

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
            if (ActiveUser.Role == "admin")
            {
                _artifactDao.BuyArtifact(id, ActiveUser.Id);
            }
        }

        public void UseArtifact(int id)
        {
            if (ActiveUser.Role == "admin")
            {
                _artifactDao.UseArtifact(id);
            }
        }

        public void DeleteStudentArtifact(int id)
        {
            if (ActiveUser.Role == "admin")
            {
                _artifactDao.DeleteStudentArtifact(id);
            }
        }

        private void CheckAffordability(IEnumerable<Artifact> artifacts)
        {
            var ActiveStudent = (Student) ActiveUser;

            foreach (var artifact in artifacts)
            {
                artifact.IsAffordable = ActiveStudent.Coolcoins >= artifact.Price;
            }
        }
    }
}