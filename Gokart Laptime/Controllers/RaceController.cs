using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Gokart_Laptime.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Gokart_Laptime.Controllers
{
    [Authorize]
    public class RaceController : Controller
    {
        private readonly IRaceTrackDAO raceTrackDAO;
        private readonly IRaceDAO raceDAO;
        private readonly IRacerDAO racerDAO;
        private readonly ILapTimeDAO lapTimeDAO;
        private readonly IHtmlLocalizer<RaceController> localizer;

        public RaceController(IRaceDAO raceDAO, IRaceTrackDAO raceTrackDAO, IRacerDAO racerDAO, ILapTimeDAO lapTimeDAO, IHtmlLocalizer<RaceController> localizer)
        {
            this.raceDAO = raceDAO;
            this.raceTrackDAO = raceTrackDAO;
            this.racerDAO = racerDAO;
            this.lapTimeDAO = lapTimeDAO;
            this.localizer = localizer;

        }
        // GET: RaceController
        public ActionResult Index(string? searchRaceTrack)
        {
            List<RaceModel> races = raceDAO.GetAllRaces();
            races.ForEach(race => race.Racers = racerDAO.GetRacersByRaceID(race.RaceId));
            foreach (RaceModel race in races)
            {
                race.Racers = racerDAO.GetRacersByRaceID(race.RaceId);

                foreach (RacerModel racer  in race.Racers)
                {
                    racer.Laptimes = lapTimeDAO.GetRacerLaptimeByRaceAndRacerId(race.RaceId, racer.RacerId);
                }
            }

            if (searchRaceTrack is not null) races = races.FindAll(race => race.RaceTrackName.ToLower().Contains(searchRaceTrack));

            ViewBag.Information = TempData["Information"];
            return View(races);
        }

        // GET: RaceController/Details/5
        public ActionResult Details(int id)
        {
            RaceModel race = raceDAO.GetRaceById(id);
            if (race is not null)
            {
                race.Racers = racerDAO.GetRacersByRaceID(id);
                race.Racers.ForEach(racer => racer.Laptimes = lapTimeDAO.GetRacerLaptimeByRaceAndRacerId(id, racer.RacerId));


                Random random = new Random();

                if(race.Racers.Count > 0)
                {
                    var data = new
                    {
                        labels = Enumerable.Range(1, race.Racers.Select(x => x.Laptimes.Count).Max()),
                        datasets = race.Racers.Select(racer => new
                        {
                            label = racer.RacerName,
                            data = racer.Laptimes.Select(lapTime => lapTime.LapTime.TotalSeconds),
                            borderColor = string.Format("#{0:X6}", random.Next(0x1000000)),
                        })
                    };
                    string output = JsonConvert.SerializeObject(data);
                    ViewBag.Data = output;

                }



                ViewBag.Information = TempData["Information"];
                return View(race);
            }
            else
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["CantFindRace"].Value });
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: RaceController/Create
        public ActionResult Create()
        {
            var raceTrackList = raceDAO.RaceTracksList();
            ViewData["RaceTracks"] = new SelectList(raceTrackList, "Key", "Value");
            return View();
        }

        // POST: RaceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RaceModel race)
        {
            try
            {
                race.Created_By = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);
                if (!ModelState.IsValid) return View(race);

                int raceId = raceDAO.AddRace(race);

                if (raceId != -1)
                {
                    racerDAO.AddRacers(new List<int> { Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value) }, raceId);
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "success", Message = localizer["RaceAddSuccess"].Value });
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RaceAddFail"].Value });
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            RaceModel race = raceDAO.GetRaceById(id);
            if (race is null)
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["CantFindRace"].Value });
                return RedirectToAction(nameof(Index));
  
            }
            else if(race.Created_By != Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value))
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["NotAllowed"].Value });
                return RedirectToAction(nameof(Index));
            }

            var raceTrackList = raceDAO.RaceTracksList();
            ViewData["RaceTracks"] = new SelectList(raceTrackList, "Key", "Value");
            return View(race);
        }

        // POST: RaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RaceModel race)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var raceTrackList = raceDAO.RaceTracksList();
                    ViewData["RaceTracks"] = new SelectList(raceTrackList, "Key", "Value");
                    return View(race);
                }

                if (raceDAO.UpdateRace(race))
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "success", Message = localizer["RaceUpdateSuccess"].Value });
                    return RedirectToAction(nameof(Index));

                }

                else
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RaceUpdateFail"].Value });
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RaceUpdateFail"].Value });
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Racers(int raceId)
        {
            RaceModel race = raceDAO.GetRaceById(raceId);

            if (race.Created_By != Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value))
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["NotAllowed"].Value });
                return RedirectToAction("Details", new { id = raceId });
            }

            RaceRacersViewModel raceRacersViewModel = new RaceRacersViewModel
            {
                Race = race,
                IncludedRacers = racerDAO.GetRacersByRaceID(raceId),
                NotIncludedRacers = racerDAO.GetAllRacers(raceId)
            };
            //ViewBag.Information = TempData["Information"];
            return View(raceRacersViewModel);
        }

        public ActionResult AddRacers(int raceId, List<int> racersId)
        {
            try
            {
                if (racersId .Count > 0)
                {
                    racerDAO.AddRacers(racersId, raceId);
                    ViewBag.Information = JsonConvert.SerializeObject(new { Type = "success", Message = localizer["RacersAddedSuccess"].Value });
                }
                else
                {
                    ViewBag.Information = JsonConvert.SerializeObject(new { Type = "warning", Message = localizer["SelectRacer"].Value });
                }

                RaceRacersViewModel raceRacersViewModel = new RaceRacersViewModel
                {
                    Race = raceDAO.GetRaceById(raceId),
                    IncludedRacers = racerDAO.GetRacersByRaceID(raceId),
                    NotIncludedRacers = racerDAO.GetAllRacers(raceId)
                };

                return View("Racers", raceRacersViewModel);

            }
            catch (Exception)
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RacersAddedFail"].Value });
                return RedirectToAction("Details", new { id = raceId });
            }
        }

        public ActionResult RemoveRacer(int racerId, int raceId)
        {
            if (racerDAO.racerHasLaptimes(raceId, racerId))
            {
                ViewBag.Information = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RacerDeleteFail"].Value });
            }
            else
            {
                racerDAO.RemoveRacer(raceId, racerId);
                ViewBag.Information = JsonConvert.SerializeObject(new { Type = "success", Message = localizer["RacerRemoved"].Value });
            }

            RaceRacersViewModel raceRacersViewModel = new RaceRacersViewModel
            {
                Race = raceDAO.GetRaceById(raceId),
                IncludedRacers = racerDAO.GetRacersByRaceID(raceId),
                NotIncludedRacers = racerDAO.GetAllRacers(raceId)
            };

            return View("Racers", raceRacersViewModel);

        }

        [HttpGet]
        public ActionResult LapTimes(int raceId)
        {
            
            if (!racerDAO.GetRacersByRaceID(raceId).Any(r => r.RacerId == Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value)))
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RacerNotAdded"].Value });
                return RedirectToAction("Details", new { id = raceId });
            }

            RaceLaptimesViewModel raceLaptimesViewModel = new RaceLaptimesViewModel
            {
                Race = raceDAO.GetRaceById(raceId),
                Laptimes = lapTimeDAO.GetRacerLaptimeByRaceAndRacerId(raceId, Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value))
            };
            return View(raceLaptimesViewModel);
        }

        [HttpPost]
        public ActionResult LapTimes(RaceLaptimesViewModel raceLaptimesViewModel)
        {
            raceLaptimesViewModel.RacerId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value);
            
            if (raceLaptimesViewModel.Laptimes is null) raceLaptimesViewModel.Laptimes = new List<LaptimeModel>();

            lapTimeDAO.ManageLapTimes(raceLaptimesViewModel);

            return RedirectToAction("Details", new { Id = raceLaptimesViewModel.RaceId } );
        }

     
    }
}

