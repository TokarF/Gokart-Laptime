using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class RacerModel
    {
        [Display(Name = "RacerID")]
        [DisplayFormat(DataFormatString = "{0:000}")]
        public int RacerId { get; set; }

        public int? RaceId { get; set; }

        [Display(Name = "Name")]
        public string? RacerName { get; set; }

        [Display(Name = "Laptimes")]
        public List<LaptimeModel>? Laptimes { get; set; }

        [Display(Name = "Average Laptime")]
        [DisplayFormat(DataFormatString = "{0:mm\\:ss\\.fff}")]
        public TimeSpan? AverageLapTime
        {
            get 
            {
                if (this.Laptimes.Count > 0)
                {
                    return TimeSpan.FromTicks((long)this.Laptimes.Select(x => x.LapTime.Ticks).Average());
                }
                else
                {
                    return null;
                }
                
            }
        }

        [Display(Name = "Best Laptime")]
        [DisplayFormat(DataFormatString = "{0:mm\\:ss\\.fff}")]
        public TimeSpan? BestLapTime
        {
            get
            {
                if (this.Laptimes.Count > 0)
                {
                    return this.Laptimes.Select(x => x.LapTime).Min();
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
