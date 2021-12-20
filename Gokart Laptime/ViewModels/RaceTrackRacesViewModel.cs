using Gokart_Laptime.Models;
using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.ViewModels
{
    public class RaceTrackRacesViewModel
    {
        [Display(Name = "Racetrack")]
        public RaceTrackModel RaceTrack { get; set; }

        [Display(Name = "Races")]
        public List<RaceModel> Races { get; set; }

    }
}
