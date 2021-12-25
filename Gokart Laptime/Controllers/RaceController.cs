using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Gokart_Laptime.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Gokart_Laptime.Controllers
{
    [Authorize]
    public class RaceController : Controller
    {
        private readonly IRaceDAO raceDAO;
        private readonly IRaceTrackDAO raceTrackDAO;
        public RaceController(IRaceDAO raceDAO, IRaceTrackDAO raceTrackDAO)
        {
            this.raceDAO = raceDAO;
            this.raceTrackDAO = raceTrackDAO;
        }
        // GET: RaceController
        public ActionResult Index()
        {
            List<RaceModel> races = raceDAO.GetAllRaces();

            ViewBag.Information = TempData["Information"];
            return View(races);
        }

        // GET: RaceController/Details/5
        public ActionResult Details(int id)
        {
            RaceModel race = raceDAO.GetRaceById(id);
            if (race is not null)
            {
                return View(race);
            }
            else
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "Sorry, something went wrong, we can't find the race!" });
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
                    raceDAO.AddRacer(Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value), raceId);
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "success", Message = "Race has been successfully added!" });
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "Sorry, something went wrong, couldn't add race!" });
                    return RedirectToAction(nameof(Index));

                }

            }
            catch
            {
                return View();
            }
        }

        // GET: RaceController/Edit/5
        public ActionResult Edit(int id)
        {
            RaceModel race = raceDAO.GetRaceById(id);
            if (race is null)
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "Race is not found!" });
                return RedirectToAction(nameof(Index));
  
            }
            else if(race.Created_By != Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value))
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "You are not allowed to do that!" });
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
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "success", Message = "Race has been successfully updated!" });
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "Sorry, something went wrong, couldn't update racetrack!" });
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "Sorry, something went wrong, couldn't update racetrack!" });
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Racers(int id)
        {
            RaceModel race = raceDAO.GetRaceById(id);

            if (race.Created_By != Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value))
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "You are not allowed to do that!" });
                return RedirectToAction(nameof(Index));
            }

            RaceRacersViewModel raceRacersViewModel = new RaceRacersViewModel
            {
                Race = race,
                IncludedRacers = raceDAO.GetRaceRacers(id),
                NotIncludedRacers = raceDAO.GetAllRacers(id)
            };
            //ViewBag.Information = TempData["Information"];
            return View(raceRacersViewModel);
        }

        public PartialViewResult AddRacer(int selectedRacerId, int raceId)
        {
            raceDAO.AddRacer(selectedRacerId, raceId);
            
            RaceModel race = raceDAO.GetRaceById(raceId);


            RaceRacersViewModel raceRacersViewModel = new RaceRacersViewModel
            {
                Race = race,
                IncludedRacers = raceDAO.GetRaceRacers(raceId),
                NotIncludedRacers = raceDAO.GetAllRacers(raceId)
            };

            return PartialView("_RaceRacers", raceRacersViewModel);

        }

        public PartialViewResult RemoveRacer(int racerId, int raceId)
        {
            raceDAO.RemoveRacer(raceId, racerId);

            RaceModel race = raceDAO.GetRaceById(raceId);


            RaceRacersViewModel raceRacersViewModel = new RaceRacersViewModel
            {
                Race = race,
                IncludedRacers = raceDAO.GetRaceRacers(raceId),
                NotIncludedRacers = raceDAO.GetAllRacers(raceId)
            };
            

            return PartialView("_RaceRacers", raceRacersViewModel);

        }

        // GET: RaceController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RaceController/Delete/5
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

