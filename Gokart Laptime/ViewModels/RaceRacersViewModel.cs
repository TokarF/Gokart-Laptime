using Gokart_Laptime.Models;

namespace Gokart_Laptime.ViewModels
{
    public class RaceRacersViewModel
    {
        public RaceModel Race { get; set; }
        public List<RacerModel>? NotIncludedRacers { get; set; } 
        public List<RacerModel>? IncludedRacers { get; set; }
    }
}
