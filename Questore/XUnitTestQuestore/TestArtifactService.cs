using FizzWare.NBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Questore.Data.Interfaces;
using Questore.Data.Models;
using Questore.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;



namespace XUnitTestQuestore
{
    public class TestArtifactService
    {
        private ICollection<Artifact> _artifacts;

        public TestArtifactService()
        {

        }

        public void Dispose()
        {
            _artifacts.Clear();
        }

        private void SeedArtifacts()
        {
            _artifacts = Builder<Artifact>
                .CreateListOfSize(100)
                .Build();
        }

        private void SeedArtifacts_Where_OneArtifactIsWorth100Coins()
        {
            var priceGenerator = new RandomGenerator();
            _artifacts = Builder<Artifact>
                .CreateListOfSize(100)
                .TheFirst(1)
                .With(a => a.Price = 100)
                .TheRest()
                .With(a => a.Price = priceGenerator.Next(200, 1000))
                .Build();
        }

        [Fact]
        public void GetArtifact_AfterAddingArtifact_ContainsArtifact()
        {
            //given
            SeedArtifacts();
            var artifactDao = ArtifactDaoMock();

            var artifact = Builder<Artifact>.CreateNew()
                .With(artifact => artifact.Id = 101)
                .Build();

            //when
            artifactDao.AddArtifact(artifact);
            var retreievedArtifact = artifactDao.GetArtifact(101);

            //then
            Assert.Contains(artifact, _artifacts);
            Assert.Equal(artifact, retreievedArtifact);

        }

        [Fact]
        public void GetAffordableArtifacts_Having100Coins_AffordsOneArtifact()
        {
            //given
            SeedArtifacts_Where_OneArtifactIsWorth100Coins();
            var artifactDao = ArtifactDaoMock();
            var serviceProvider = ServiceProviderMock(HttpContextAccessorMock(
                new Student()
                {
                    Coolcoins = 100
                }));
            var artifactService = new ArtifactService(serviceProvider, artifactDao);


            //when
            var sut = artifactService.MarkAffordableArtifacts();
            var affordableCount = sut.Count(a => a.IsAffordable == true);
            Assert.Equal(1, affordableCount);

        }

        private IArtifactDAO ArtifactDaoMock()
        {
            var mock = new Mock<IArtifactDAO>();
            mock.Setup(mock => mock.GetArtifacts())
                .Returns(_artifacts);
            mock.Setup(mock => mock.GetArtifact(It.IsAny<int>()))
                .Returns((int id) => _artifacts.FirstOrDefault(art => art.Id == id));
            mock.Setup(mock => mock.AddArtifact(It.IsAny<Artifact>()))
                .Callback((Artifact art) => _artifacts.Add(art));
            return mock.Object;
        }

        //ServiceProvider + HttpContextAccessor + Session mocks
        private IServiceProvider ServiceProviderMock(IHttpContextAccessor mockHttpContextAccessor)
        {
            //Arrange
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpContextAccessor)))
                .Returns(mockHttpContextAccessor);

            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            return serviceProvider.Object;
        }
        private IHttpContextAccessor HttpContextAccessorMock(Student student)
        {

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext.Session).Returns(SessionMock(student));
            return mockHttpContextAccessor.Object;
        }
        private static ISession SessionMock(Student student)
        {
            MockHttpSession httpcontext = new MockHttpSession();
            httpcontext.SetString("user", JsonSerializer.Serialize<object>(student));
            return httpcontext;
        }
    }
}
