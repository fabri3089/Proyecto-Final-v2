using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Models.Repositories;
using System.Configuration;
using PagedList;

namespace ProyectoFinal.Controllers
{
    [HandleError()]
    public class ActivitySchedulesController : Controller
    {
        #region Properties
        private IActivityScheduleRepository activityScheduleRepository;
        private IActivityRepository activityRepository;
        #endregion

        #region Constructors
        public ActivitySchedulesController()
        {
            this.activityScheduleRepository = new ActivityScheduleRepository(new GymContext());
            this.activityRepository = new ActivityRepository(new GymContext());
        }

        public ActivitySchedulesController(IActivityScheduleRepository activityScheduleRepository, IActivityRepository activityRepository)
        {
            this.activityScheduleRepository = activityScheduleRepository;
            this.activityRepository = activityRepository;
        }
        #endregion

        // GET: ActivitySchedules
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var activitySchedules = activityScheduleRepository.GetActivitySchedules();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                activitySchedules = activitySchedules.Where(c => c.Activity.Name.ToLower().Contains(searchString));
            }
            #endregion

            #region OrderBy
            ViewBag.ActivityNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DaySortParm = sortOrder == "day_asc" ? "day_desc" : "day_asc";
            ViewBag.HourFromSortParm = sortOrder == "hourFrom_asc" ? "hourFrom_desc" : "hourFrom_asc";
            ViewBag.HourToSortParm = sortOrder == "hourTo_asc" ? "hourTo_desc" : "hourTo_asc";

            switch (sortOrder)
            {
                case "name_desc":
                    activitySchedules = activitySchedules.OrderByDescending(a => a.Activity.Name);
                    break;
                case "day_desc":
                    activitySchedules = activitySchedules.OrderByDescending(a => a.Day);
                    break;
                case "day_asc":
                    activitySchedules = activitySchedules.OrderBy(a => a.Day);
                    break;
                case "hourFrom_desc":
                    activitySchedules = activitySchedules.OrderByDescending(a => a.HourFrom);
                    break;
                case "hourFrom_asc":
                    activitySchedules = activitySchedules.OrderBy(a => a.HourFrom);
                    break;
                case "hourTo_desc":
                    activitySchedules = activitySchedules.OrderByDescending(a => a.HourTo);
                    break;
                case "hourTo_asc":
                    activitySchedules = activitySchedules.OrderBy(a => a.HourTo);
                    break;
                default:
                    activitySchedules = activitySchedules.OrderBy(a => a.Activity.Name);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(activitySchedules.ToPagedList(pageNumber, pageSize));
        }

        // GET: ActivitySchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivitySchedule activitySchedule = activityScheduleRepository.GetActivityScheduleByID((int)id);
            if (activitySchedule == null)
            {
                return HttpNotFound();
            }
            return View(activitySchedule);
        }

        // GET: ActivitySchedules/Create
        public ActionResult Create()
        {
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name");
            return View();
        }

        // POST: ActivitySchedules/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActivityScheduleID,Day,HourFrom,HourTo,ActivityID")] ActivitySchedule activitySchedule)
        {
            if (ModelState.IsValid)
            {
                activityScheduleRepository.InsertActivitySchedule(activitySchedule);
                activityScheduleRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", activitySchedule.ActivityID);
            return View(activitySchedule);
        }

        // GET: ActivitySchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivitySchedule activitySchedule = activityScheduleRepository.GetActivityScheduleByID((int)id);
            if (activitySchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", activitySchedule.ActivityID);
            return View(activitySchedule);
        }

        // POST: ActivitySchedules/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActivityScheduleID,Day,HourFrom,HourTo,ActivityID")] ActivitySchedule activitySchedule)
        {
            if (ModelState.IsValid)
            {
                activityScheduleRepository.UpdateActivitySchedule(activitySchedule);
                activityScheduleRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", activitySchedule.ActivityID);
            return View(activitySchedule);
        }

        // GET: ActivitySchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivitySchedule activitySchedule = activityScheduleRepository.GetActivityScheduleByID((int)id);
            if (activitySchedule == null)
            {
                return HttpNotFound();
            }
            return View(activitySchedule);
        }

        // POST: ActivitySchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivitySchedule activitySchedule = activityScheduleRepository.GetActivityScheduleByID((int)id);
            activityScheduleRepository.DeleteActivitySchedule((int)id);
            activityScheduleRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                activityScheduleRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
