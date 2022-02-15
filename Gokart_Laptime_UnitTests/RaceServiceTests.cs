using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Gokart_Laptime_UnitTests
{
    [TestFixture]
    public class RaceServiceTests
    {
        private Mock<IRaceDAO> mockRaceDAO;

        [SetUp]
        public void SetUp()
        {
            mockRaceDAO = new Mock<IRaceDAO>();

        }


        [Test]
        public void GetAllRaces_WithoutAddedRaces_ReturnEmptyList()
        {


            mockRaceDAO.Setup(x => x.GetAllRaces()).Returns(GetSampleRaces());

            var result = mockRaceDAO.Object.GetAllRaces();
            var expected = GetSampleRaces();

            Assert.AreEqual(expected.Count, result.Count);

        }

        [Test]
        public void AddRace_WithValidRaceModel_AddedToList()
        {
            RaceModel race = new RaceModel() { RaceId = 1, RaceTrackId = 2, RaceDate = new System.DateTime(2020, 12, 05), Created_By = 1 };

            mockRaceDAO.Setup(x => x.AddRace(race));


            var result = mockRaceDAO.Object.AddRace(race);

            mockRaceDAO.Verify(x => x.AddRace(race), Times.Once);

        }


        [Test]
        public void Update_WithValidRaceModel_UpdatedList()
        {
            RaceModel race = new RaceModel() { RaceId = 1, RaceTrackId = 2, RaceDate = new System.DateTime(2020, 12, 05), Created_By = 1 };

            mockRaceDAO.Setup(x => x.UpdateRace(race));


            var result = mockRaceDAO.Object.UpdateRace(race);

            mockRaceDAO.Verify(x => x.UpdateRace(race), Times.Once);

        }

        private List<RaceModel> GetSampleRaces()
        {
            List<RaceModel> races = new List<RaceModel>()
            {
                 new RaceModel() { RaceId = 1,  RaceTrackId = 2, RaceDate = new System.DateTime(2020, 06, 05), Created_By = 1 },
                 new RaceModel() { RaceId = 2,  RaceTrackId = 1, RaceDate = new System.DateTime(2020, 12, 10), Created_By = 2 },
            };

            return races;
        }

    }
}
