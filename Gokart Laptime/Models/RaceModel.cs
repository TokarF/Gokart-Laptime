using System.ComponentModel.DataAnnotations;

namespace Gokart_Laptime.Models
{
    public class RaceModel
    {
        [Key]
        [Display(Name = "Race")]
        [DisplayFormat(DataFormatString = "{0:00000}")]
        public int RaceId { get; set; }

        
        [Display(Name = "RaceTrackID")]
        [Required(ErrorMessage = "The racetrack is required")]
        public int RaceTrackId { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "The race date is required")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RaceDate { get; set; }


        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "The racetrack description has to be between 10 - 300 charaters")]
        [DisplayFormat(NullDisplayText = "N/A")]
        public string? Description { get; set; }

        [Display(Name = "Track")]
        public string? RaceTrackName { get; set; }

        [Display(Name = "Created by")]
        public int Created_By { get; set; }

        [Display(Name = "Racers")]
        public List<RacerModel>? Racers { get; set; } = null;

  
    }
}