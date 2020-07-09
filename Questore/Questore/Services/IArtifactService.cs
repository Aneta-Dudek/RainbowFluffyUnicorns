using Questore.Models;
using System.Collections.Generic;

namespace Questore.Services
{
    public interface IArtifactService
    {
        public IEnumerable<Artifact> GetAffordableArtifacts();

        public void BuyArtifact(int id);

        public void UseArtifact(int id);

        public void DeleteStudentArtifact(int id);
    }
}