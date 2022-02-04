using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class LaptimeModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "RaceID")]
        public int RaceId { get; set; }

        [Display(Name = "RacerID")]
        [DisplayFormat(DataFormatString = "{0:000}")]
        public int RacerId { get; set; }

        [Display(Name = "Lap")]
        public int Lap { get; set; }

        [Display(Name = "Laptime")]
        [DisplayFormat(DataFormatString = "{0:mm\\:ss\\.fff}")]
        public TimeSpan LapTime { get; set; }

        [Display(Name = "Difference")]
        public TimeSpan Difference(TimeSpan currLaptime, TimeSpan prevLapTime)
        {
            return currLaptime - prevLapTime;
        }
    }
}
