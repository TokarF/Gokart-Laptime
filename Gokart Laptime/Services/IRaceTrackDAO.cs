using Gokart_Laptime.Models;

namespace Gokart_Laptime.Services
{
    public interface IRaceTrackDAO
    {
        List<RaceTrackModel> GetAllRaceTracks();
        int AddRaceTrack(RaceTrackModel raceTrackModel);

        RaceTrackModel GetRaceTrackById(int id);
        bool UpdateRaceTrack(RaceTrackModel raceTrack);
    }
}
