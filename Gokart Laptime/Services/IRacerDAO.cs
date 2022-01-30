using Gokart_Laptime.Models;

namespace Gokart_Laptime.Services
{
    public interface IRacerDAO
    {
        List<RacerModel> GetRacersByRaceID(int raceId);

        List<RacerModel> GetAllRacers(int raceId);

        void AddRacers(List<int> racersId, int raceId);

        void RemoveRacer(int raceId, int racerId);
        bool racerHasLaptimes(int raceId, int racerId);

    }
}
