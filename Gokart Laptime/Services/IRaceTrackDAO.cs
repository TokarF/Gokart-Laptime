using Gokart_Laptime.Models;

namespace Gokart_Laptime.Services
{
    public interface IRaceTrackDAO
    {
        List<RaceTrackModel> GetAllRaceTracks();
        int AddRaceTrack(RaceTrackModel raceTrack);

        RaceTrackModel GetRaceTrackById(int id);
        bool UpdateRaceTrack(RaceTrackModel raceTrack);
    }
}
