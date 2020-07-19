namespace Questore.Data.Persistence
{
    public static class DBUtilities
    {
        public enum StudentEnum { Id, FirstName, LastName, Coolcoins, Experience, ImageUrl, PublicImageId, Credentials }
        public enum AdminEnum { Id, FirstName, LastName, ImageUrl, PublicImageId, Credentials }
        public enum ArtifactEnum { Id, Name, Description, ImageUrl, Price, IsUsed, StudentArtifactId }
        public enum ClassEnum { Id, Name, DateStarted, ImageUrl }
        public enum TeamEnum { Id, Name, ImageUrl }
        public enum CategoryEnum { Id, Name }
        public enum TitleEnum { Id, Name, Threshold }
        public enum QuestEnum { Id, Name, Description, Reward, ImageUrl }
        public enum StudentDetailEnum { Id, Name, Content }
    }
}