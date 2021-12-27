using Gokart_Laptime.Models;

namespace Gokart_Laptime.Services
{
    public interface ILapTimeDAO
    {
        List<LaptimeModel> GetRacerLaptimeByRaceAndRacerId(int raceId, int racerId);

        void UpdateLapTimes(List<LaptimeModel> lapTimesToUpdate);
        void RemoveLapTimes(List<LaptimeModel> lapTimesToDelete);
    }
}
