using Gokart_Laptime.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gokart_Laptime.Controllers
{
    public class RaceTrackController : Controller
    {
        private readonly RaceTrackDAO raceTrackDAO;
        public RaceTrackController(IConfiguration configuration)
        {
            raceTrackDAO = new RaceTrackDAO(configuration);
        }
        // GET: RaceTrackController
        [Authorize]
        public ActionResult Index()
        {

            try
            {
                List<RaceTrackModel> raceTracks = raceTrackDAO.GetAllRaceTracks();
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
            return View();
        }

        // GET: RaceTrackController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RaceTrackController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: RaceTrackController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RaceTrackController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
