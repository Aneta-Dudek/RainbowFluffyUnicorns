using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using FizzWare.NBuilder;

using Questore.Models;
using Questore.Persistence;

using Xunit;

namespace Questore.MockingExamples
{
    public class MockExamples : IDisposable
    {
        private readonly ICollection<Artifact> artifacts;

        private readonly ICollection<Quest> quests;

        public MockExamples()
        {
            artifacts = Builder<Artifact>.CreateListOfSize(100).Build();
            quests = Builder<Quest>.CreateListOfSize(100).Build();
        }

        public void Dispose()
        {
            artifacts.Clear();
            quests.Clear();
        }

        [Fact]
        public void MockExample()
        {
            var dao = GetDaoMock(); // see comments

            var artifact = Builder<Artifact>.CreateNew()
                .With(artifact => artifact.Id = 101)
                .Build();

            dao.AddArtifact(artifact);

            Assert.Contains(artifact, artifacts);

            var retrievedArtifact = dao.GetArtifact(101);

            Assert.Equal(artifact, retrievedArtifact);

            Assert.Throws<InvalidOperationException>(() => dao.DeleteArtifact(1));
        }

        [Fact]
        public void DoNotDoThisOrIWillFindYouAndStrangleYouWithATowel()
        {
            var dao = GetDaoMock();

            var artifact = Builder<Artifact>.CreateNew()
                .With(artifact => artifact.Id = 101)
                .Build();

            dao.AddArtifact(artifact);

            // if you do this, I will slaughter you. No matter what!
            Mock.Get(dao).Verify(mock => mock.AddArtifact(It.IsAny<Artifact>()), Times.Once);
        }

        [Fact]
        public void MockingMultipleInterfaces()
        {
            // start setting up mock - as usual...
            var mock = new Mock<IArtifactDAO>();
            mock.Setup(mock => mock.GetArtifact(It.IsAny<int>()))
                .Returns((int id) => artifacts.FirstOrDefault(art => art.Id == id));

            // 'cast' mock to setup another interface
            var mockOfQuestDao = mock.As<IQuestDAO>();
            mockOfQuestDao.Setup(m => m.GetQuest(It.IsAny<int>()))
                .Returns((int id) => quests.FirstOrDefault(q => q.Id == id));

            // here we have mock of IQuestDAO
            var questDao = mockOfQuestDao.Object;

            // check behavior of the mock
            Assert.NotNull(questDao.GetQuest(1));

            // check whether mock is an instance of IArtifactDAO
            Assert.True(questDao is IArtifactDAO);

            var artifactDao = (IArtifactDAO)questDao;

            // check mock behavior - this time as an IArtifactDAO
            Assert.NotNull(artifactDao.GetArtifact(1));
        }

        private IArtifactDAO GetDaoMock()
        {
            // start setting up a new mock of IArtifactDAO
            var mock = new Mock<IArtifactDAO>();

            // setup return values
            mock.Setup(mock => mock.GetArtifact(It.IsAny<int>())) // It.IsAny<T> is a 'wildcard' parameter of T type
                .Returns((int id) => artifacts.FirstOrDefault(art => art.Id == id)); // return value - FirstOrDefault with matching id

            // setup callbacks (aka: do sth after this method is called)
            mock.Setup(mock => mock.AddArtifact(It.IsAny<Artifact>())) // again, wildcard parameter
                .Callback((Artifact art) => artifacts.Add(art)); // callback - what should be done after calling this method

            mock.Setup(m => m.DeleteArtifact(It.IsAny<int>())) // wildcard
                .Throws(new InvalidOperationException("I'm thrown because I was told to be thrown like a dwarf!")); // throw me an exception; 'cause I'm execptional

            return mock.Object; // return a mock
        }

        [Fact]
        public void MagicalFunctionalThings()
        {
            // if you can understand this implementation, I owe you one beer :D
            var pascalTriangleInOneLine = new[] { 1 }.Iterate(item => item.Append(0).Zip(item.Prepend(0), (item1, item2) => item1 + item2));

            Assert.Equal(new[] { 1, 2, 1 }, pascalTriangleInOneLine.Skip(2).First());
            Assert.Equal(new[] { 1, 3, 3, 1 }, pascalTriangleInOneLine.Skip(3).First());
        }
    }
}
