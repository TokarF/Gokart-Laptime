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

        bool CreatedByLoggedInUser(int raceId, int racerId);
    }
}
