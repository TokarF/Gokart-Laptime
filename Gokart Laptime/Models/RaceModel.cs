using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class RaceModel
    {
        [Key]
        [Display(Name = "Race")]
        [DisplayFormat(DataFormatString = "{0:00000}")]
        public int RaceId { get; set; }

        
        [Display(Name = "RaceTrack")]
        [Required(ErrorMessage = "RacetrackRequired")]
        public int RaceTrackId { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "RacedateRequired")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RaceDate { get; set; }


        [Display(Name = "Description", Prompt = "DescriptionPlaceHolder")]
        [DataType(DataType.MultilineText)]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "RacetrackDescriptionLength")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? Description { get; set; }

        [Display(Name = "RaceTrackName")]
        public string? RaceTrackName { get; set; }

        [Display(Name = "Created by")]
        public int Created_By { get; set; }

        [Display(Name = "Racers")]
        public List<RacerModel>? Racers { get; set; }

        [Display(Name = "Best Lap")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? RaceBestLapTime
        {
            get 
            {
                if (Racers is not null)
                {
                    var bestLap = Racers.Where(racer => racer.Laptimes.Count > 0)
                                         .Select(racer => new
                                         {
                                             RacerName = racer.RacerName,
                                             LapTime = racer.BestLapTime
                                         }).MinBy(x => x.LapTime);

                    return string.Format("{0} - {1:mm\\:ss\\.fff}", bestLap?.RacerName, bestLap?.LapTime);
                }
                else
                {
                    return null;
                }
            }
        }

    }
}