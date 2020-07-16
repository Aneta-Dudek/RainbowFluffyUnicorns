using System.Collections.Generic;
using Questore.Data.Models;

namespace Questore.Services.Interfaces
{
    public interface IArtifactService
    {
        public IEnumerable<Artifact> GetAffordableArtifacts();

        public void BuyArtifact(int id);

        public void UseArtifact(int id);

        public void DeleteStudentArtifact(int id);
    }
}