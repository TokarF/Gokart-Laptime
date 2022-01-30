using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class RaceTrackModel
    {
        [Key]
        [Display(Name = "#")]
        [DisplayFormat(DataFormatString = "{0:000}")]
        public int RaceTrackId { get; set; }

        [Display(Name = "Racetrack name")]
        [Required(ErrorMessage = "The racetrack name is required")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 100, MinimumLength = 10, ErrorMessage = "The racetrack name has to be between 10 - 100 charaters")]
        public string RaceTrackName { get; set; }


        [Display(Name = "Length")]
        [Required(ErrorMessage = "The racetrack length is required")]
        [Range(minimum: 100, maximum: 5000, ErrorMessage = "The racetrack length has to be between 100 - 5000")]
        [DisplayFormat(DataFormatString = "{0,4} m")]
        public int Length { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The racetrack address is required")]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 200, MinimumLength = 10, ErrorMessage = "The racetrack address has to be between 10 - 200 charaters")]
        public string Address { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "The racetrack description has to be between 10 - 300 charaters")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? Description { get; set; }

        [Display(Name = "Races")]
        public List<RaceModel>? Races { get; set; }

        [Display(Name = "Best Lap")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? RaceTrackBestLapTime
        {
            get
            {
                if (Races is not null)
                {

                    var raceTrackBestLap = (from race in Races
                                            from racer in race.Racers
                                            where race.Racers is not null
                                            from lapTimes in racer.Laptimes
                                            where racer.Laptimes is not null
                                            select new { RacerName = racer.RacerName, RaceDate = race.RaceDate, LapTime = lapTimes.LapTime }).MinBy(x => x.LapTime);

                    return string.Format("{0:yyyy-MM-dd} - {1} - {2:mm\\:ss\\.fff}", raceTrackBestLap.RaceDate, raceTrackBestLap.RacerName, raceTrackBestLap.LapTime);

                }
                else
                {
                    return null;
                }
            }
        }


    }
}
