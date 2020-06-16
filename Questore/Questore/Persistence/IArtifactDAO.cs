﻿using System.Collections.Generic;
using Questore.Models;

namespace Questore.Persistence
{
    public interface IArtifactDAO
    {
        IEnumerable<Artifact> GetArtifacts();
        Artifact GetArtifact(int id);
        void AddArtifact(Artifact artifact);
        void UpdateArtifact(int id, Artifact updatedArtifact);
        void DeleteArtifact(int id);
    }
}