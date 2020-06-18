namespace Questore.Persistence
{
    public static class DBUtilities
    {
        public enum StudentEnum { Id, FirstName, LastName, Email, Coolcoins, Experience, ImageUrl }

        public enum ArtifactEnum { Id, Name, Description, ImageUrl, Price, IsUsed }

        public enum ClassEnum { Id, Name, DateStarted, ImageUrl }

        public enum TeamEnum { Id, Name, ImageUrl }

        public enum CategoryEnum { Id, Name }

        public enum TitleEnum { Id, Name, Threshold }

        public enum QuestEnum { Id, Name, Description, Reward, ImageUrl }

        public enum StudentDetailEnum { Id, Name, Content }
    }
}