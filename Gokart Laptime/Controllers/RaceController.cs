using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using Gokart_Laptime.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Gokart_Laptime.Controllers
{
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
            return View();
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
                if (!ModelState.IsValid) return View(race);

                if (raceDAO.AddRace(race) != -1)
                {
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
            if (race is not null)
            {
                var raceTrackList = raceDAO.RaceTracksList();
                ViewData["RaceTracks"] = new SelectList(raceTrackList, "Key", "Value");
                return View(race);
            }
            else
            {
                TempData["Information"] = JsonConvert.SerializeObject(new { Type = "danger", Message = "Race is not found!" });
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: RaceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RaceModel race)
        {
            try
            {
                if (!ModelState.IsValid) return View(race);

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
