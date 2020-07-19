using System.Collections.Generic;
using Questore.Data.Models;

namespace Questore.Data.Interfaces
{
    public interface IArtifactDAO
    {
        IEnumerable<Artifact> GetArtifacts();
        Artifact GetArtifact(int id);
        void AddArtifact(Artifact artifact);
        void UpdateArtifact(int id, Artifact updatedArtifact);
        void DeleteArtifact(int id);
        void UseArtifact(int id);
        void BuyArtifact(int artifactId, int studentId);
        void DeleteStudentArtifact(int id);
    }
}