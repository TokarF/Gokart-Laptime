using Gokart_Laptime.Models;

namespace Gokart_Laptime.Services
{
    public interface IRaceDAO
    {
        List<RaceModel> GetAllRaces();
        int AddRace(RaceModel race);

        RaceModel GetRaceById(int id);
        bool UpdateRace(RaceModel race);
        Dictionary<int, string> RaceTracksList();

        List<RacerModel> GetRaceRacers(int raceId);
        List<RacerModel> GetAllRacers(int raceId);
        void AddRacer(int selectedRacerId, int raceId);
        void RemoveRacer(int racerId, int raceId);

        bool CreatedByLoggedInUser(int raceId, int racerId);
    }
}
