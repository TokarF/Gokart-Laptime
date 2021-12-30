using Gokart_Laptime.Models;

namespace Gokart_Laptime.ViewModels
{
    public class RaceLaptimesViewModel
    {
        public RaceModel? Race { get; set; }

        public int RaceId { get; set; }

        public int RacerId { get; set; }
        public List<LaptimeModel> Laptimes { get; set; }

    }
}
