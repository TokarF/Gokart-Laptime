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


    }
}
