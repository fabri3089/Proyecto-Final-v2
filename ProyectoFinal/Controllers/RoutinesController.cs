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
using ProyectoFinal.Filters;
using System.Configuration;
using PagedList;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin", OtherRole = "Client")]
    [HandleError()]
    public class RoutinesController : Controller
    {
        #region Properties
        private IRoutineRepository routineRepository;
        private IClientRepository clientRepository;
        #endregion

        #region Constructors
        public RoutinesController()
        {
            this.routineRepository = new RoutineRepository(new GymContext());
            this.clientRepository = new ClientRepository(new GymContext());
        }

        public RoutinesController(IRoutineRepository routineRepository, IClientRepository clientRepository)
        {
            this.routineRepository = routineRepository;
            this.clientRepository = clientRepository;
        }
        #endregion

        // GET: Routines
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

            Client currentClient = (Client)Session["User"]; 

            var routines = (Session["Role"].ToString() == "Admin") ? routineRepository.GetRoutines() : routineRepository.GetRoutinesByClientID(currentClient.ClientID);

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                routines = routines.Where(r => string.Concat(r.Client.FirstName, " ", r.Client.LastName)
                                                     .ToLower()
                                                     .Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.RoutineSortParm = sortOrder == "routine_asc" ? "routine_desc" : "routine_asc";
            ViewBag.DescSortParm = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewBag.LevelSortParm = sortOrder == "level_asc" ? "level_desc" : "level_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DaysSortParm = sortOrder == "day_asc" ? "day_desc" : "day_asc";

            switch (sortOrder)
            {
                case "name_desc":
                    routines = routines.OrderByDescending(r => r.Client.FirstName).ThenBy(r => r.Client.LastName);
                    break;
                case "routine_desc":
                    routines = routines.OrderByDescending(r => r.NameFile);
                    break;
                case "routine_asc":
                    routines = routines.OrderBy(r => r.NameFile);
                    break;
                case "description_desc":
                    routines = routines.OrderByDescending(r => r.Description);
                    break;
                case "description_asc":
                    routines = routines.OrderBy(r => r.Description);
                    break;
                case "level_desc":
                    routines = routines.OrderByDescending(r => r.Level.ToString());
                    break;
                case "level_asc":
                    routines = routines.OrderBy(r => r.Level.ToString());
                    break;
                case "status_desc":
                    routines = routines.OrderByDescending(r => r.Status.ToString());
                    break;
                case "status_asc":
                    routines = routines.OrderBy(r => r.Status.ToString());
                    break;
                case "date_desc":
                    routines = routines.OrderByDescending(r => r.CreationDate);
                    break;
                case "date_asc":
                    routines = routines.OrderBy(r => r.CreationDate);
                    break;
                case "day_desc":
                    routines = routines.OrderByDescending(r => r.DaysInWeek);
                    break;
                case "day_asc":
                    routines = routines.OrderBy(r => r.DaysInWeek);
                    break;
                default:
                    routines = routines.OrderBy(r => r.Client.FirstName).ThenBy(r => r.Client.LastName);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(routines.ToPagedList(pageNumber, pageSize));
        }

        // GET: Routines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routine routine = routineRepository.GetRoutineByID((int)id);
            if (routine == null)
            {
                return HttpNotFound();
            }
            return View(routine);
        }

        // GET: Routines/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");
            return View();
        }

        // POST: Routines/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Routine routine)
        {
            if (ModelState.IsValid)
            {
                routineRepository.InsertRoutine(routine);
                routineRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", routine.ClientID);
            return View(routine);
        }

        // GET: Routines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routine routine = routineRepository.GetRoutineByID((int)id);
            if (routine == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", routine.ClientID);
            return View(routine);
        }

        // POST: Routines/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Routine routine)
        {
            if (ModelState.IsValid)
            {
                routineRepository.UpdateRoutine(routine);
                routineRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", routine.ClientID);
            return View(routine);
        }

        // GET: Routines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routine routine = routineRepository.GetRoutineByID((int)id);
            if (routine == null)
            {
                return HttpNotFound();
            }
            return View(routine);
        }

        // POST: Routines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Routine routine = routineRepository.GetRoutineByID((int)id);
            routineRepository.DeleteRoutine((int)id);
            routineRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                routineRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
