using Questore.Data.Models;
using System.Collections.Generic;

namespace Questore.Services.Interfaces
{
    public interface IArtifactService
    {
        public IEnumerable<Artifact> GetArtifacts();

        public void BuyArtifact(int id);

        public void UseArtifact(int id);

        public void DeleteStudentArtifact(int id);
    }
}