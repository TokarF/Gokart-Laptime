using Gokart_Laptime.Models;
using Gokart_Laptime.ViewModels;

namespace Gokart_Laptime.Services
{
    public interface ILapTimeDAO
    {
        List<LaptimeModel> GetRacerLaptimeByRaceAndRacerId(int raceId, int racerId);

        List<int> GetIdsByRaceAndRacerID(int raceId, int racerId);
        void AddLapTimes(List<LaptimeModel> lapTimesToUpdate);
        void UpdateLapTimes(List<LaptimeModel> lapTimesToUpdate);
        void RemoveLapTimes(List<LaptimeModel> lapTimesToDelete);

        void ManageLapTimes(RaceLaptimesViewModel raceLaptimesViewModel);
    }
}
