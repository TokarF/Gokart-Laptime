using Gokart_Laptime.Models;

namespace Gokart_Laptime.Services
{
    public interface IRacerDAO
    {
        List<RacerModel> GetRacersByRaceID(int raceId);

        List<RacerModel> GetAllRacers(int raceId);

        void AddRacer(int selectedRacerId, int raceId);

        void RemoveRacer(int raceId, int racerId);

    }
}
