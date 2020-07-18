using Questore.Data.Models;
using System.Collections.Generic;

namespace Questore.Services.Interfaces
{
    public interface IArtifactService
    {
        public IEnumerable<Artifact> MarkAffordableArtifacts();

        public void BuyArtifact(int id);

        public void UseArtifact(int id);

        public void DeleteStudentArtifact(int id);
    }
}