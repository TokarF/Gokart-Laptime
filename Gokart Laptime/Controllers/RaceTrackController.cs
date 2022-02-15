using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Newtonsoft.Json;

namespace Gokart_Laptime.Controllers
{
    public class RaceTrackController : Controller
    {
        private readonly IRaceTrackDAO raceTrackDAO;
        private readonly IRaceDAO raceDAO;
        private readonly IRacerDAO racerDAO;
        private readonly ILapTimeDAO lapTimeDAO;
        private readonly IHtmlLocalizer<RaceTrackController> localizer;

        public RaceTrackController(IRaceTrackDAO raceTrackDAO, IRaceDAO raceDAO, IRacerDAO racerDAO, ILapTimeDAO lapTimeDAO, IHtmlLocalizer<RaceTrackController> localizer)
        {
            this.raceTrackDAO = raceTrackDAO;
            this.raceDAO = raceDAO;
            this.racerDAO = racerDAO;
            this.lapTimeDAO = lapTimeDAO;
            this.localizer = localizer;
        }
        // GET: RaceTrackController
        [Authorize]
        public ActionResult Index(string? searchRaceTrack)
        {

            try
            {
                List<RaceTrackModel> raceTracks = raceTrackDAO.GetAllRaceTracks();

                raceTracks.ForEach(raceTrack => raceTrack.Races = raceTrackDAO.GetRaceTrackRacesById(raceTrack.RaceTrackId));

                if (searchRaceTrack is not null) raceTracks = raceTracks.FindAll(race => race.RaceTrackName.ToLower().Contains(searchRaceTrack));

                ViewBag.Information = TempData["Information"];
                return View(raceTracks);
            }
            catch (Exception)
            {
                return View();
            }

        }

        // GET: RaceTrackController/Details/5
        public ActionResult Details(int id)
        {
            RaceTrackModel raceTrack = raceTrackDAO.GetRaceTrackById(id);


            if (raceTrack is not null)
            {
                raceTrack.Races = raceTrackDAO.GetRaceTrackRacesById(raceTrack.RaceTrackId).OrderByDescending(race => race.RaceId).ToList();
                
                foreach (var race in raceTrack.Races)
                {
                    race.Racers = racerDAO.GetRacersByRaceID(race.RaceId);
                    foreach (var racer in race.Racers)
                    {
                        racer.Laptimes = lapTimeDAO.GetRacerLaptimeByRaceAndRacerId(race.RaceId, racer.RacerId);
                    }
                }

                return View(raceTrack);
            }
            else
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RaceTrackNotFound"].Value });
                return RedirectToAction(nameof(Index));
            }


        }

        // GET: RaceTrackController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RaceTrackController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RaceTrackModel raceTrack)
        {
            try
            {
                if (!ModelState.IsValid) return View(raceTrack);

                if (raceTrackDAO.AddRaceTrack(raceTrack) != -1)
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "success", Message = localizer["RaceTrackAddSuccess"].Value });
                    return RedirectToAction(nameof(Index));
      
                }
                else
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RaceTrackAddFail"].Value });
                    return RedirectToAction(nameof(Index));
                }

            }
            catch
            {
                return View();
            }
        }

        // GET: RaceTrackController/Edit/5
        public ActionResult Edit(int id)
        {
            RaceTrackModel raceTrack = raceTrackDAO.GetRaceTrackById(id);
            if (raceTrack is not null)
            {
                return View(raceTrack);
            }
            else
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RaceTrackNotFound"].Value });
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RaceTrackController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RaceTrackModel raceTrack)
        {
            try
            {
                if (!ModelState.IsValid) return View(raceTrack);

                if (raceTrackDAO.UpdateRaceTrack(raceTrack))
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "success", Message = localizer["RaceTrackUpdateSuccess"].Value });
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = localizer["RaceTrackUpdateFail"].Value });
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: RaceTrackController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RaceTrackController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
