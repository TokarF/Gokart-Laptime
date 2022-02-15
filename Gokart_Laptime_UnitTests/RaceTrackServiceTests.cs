using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using NuGet.ContentModel;
using NUnit.Framework;
using System.Collections.Generic;

namespace Gokart_Laptime_UnitTests
{
    [TestFixture]
    public class RaceTrackServiceTests
    {
        private Mock<IRaceTrackDAO> mockRaceTrackDAO;

        [SetUp]
        public void SetUp()
        {
            mockRaceTrackDAO = new Mock<IRaceTrackDAO>();

        }


        [Test]
        public void GetAllRaceTracks_WithSampleRaces_ReturnList()
        {


            mockRaceTrackDAO.Setup(x => x.GetAllRaceTracks()).Returns(GetSampleRaceTracks());

            var result = mockRaceTrackDAO.Object.GetAllRaceTracks();
            var expected = GetSampleRaceTracks();

            Assert.AreEqual(expected.Count, result.Count);

        }

        [Test]
        public void AddRace_WithValidRaceModel_AddedToList()
        {
            RaceTrackModel raceTrack = new RaceTrackModel { RaceTrackId = 1, RaceTrackName = "CsepelRing", Address = "Budapest, Terelõ u. 12-14, 1211", Length = 300, };
            mockRaceTrackDAO.Setup(x => x.AddRaceTrack(raceTrack));


            var result = mockRaceTrackDAO.Object.AddRaceTrack(raceTrack);

            mockRaceTrackDAO.Verify(x => x.AddRaceTrack(raceTrack), Times.Once);

        }


        [Test]
        public void GetRaceTrack_ByRaceTrackId_ReturnRace()
        {
            mockRaceTrackDAO.Setup(x => x.GetRaceTrackById(1)).Returns(GetSampleRaceTracks()[0]);

            var result = mockRaceTrackDAO.Object.GetRaceTrackById(1);

            var expected = GetSampleRaceTracks()[0];

            bool same = result == expected;

            Assert.Multiple(() =>
            {
                Assert.AreEqual(result.RaceTrackName, expected.RaceTrackName);
                Assert.AreEqual(result.Address, expected.Address);
            });

        }



        private List<RaceTrackModel> GetSampleRaceTracks()
        {
            List<RaceTrackModel> races = new List<RaceTrackModel>()
            {
                 new RaceTrackModel { RaceTrackId = 1, RaceTrackName = "CsepelRing", Address = "Budapest, Terelõ u. 12-14, 1211", Length = 300 },
                 new RaceTrackModel { RaceTrackId = 2, RaceTrackName = "Silverkart Budapest Gokart", Address = "Budapest, Kozma u. 3/A, 1108", Length = 420 },
                 new RaceTrackModel { RaceTrackId = 3, RaceTrackName = "Budaring Gokart Pálya", Address = "Budapest, Budaörsi út HRSZ: 1092/6, 1112", Length = 400 }
            };

            return races;
        }

    }
}
