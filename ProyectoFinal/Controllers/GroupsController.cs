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
using ProyectoFinal.Filters;

namespace ProyectoFinal.Controllers
{
    [HandleError()]
    [AuthorizationPrivilege(Role = "Admin")]
    public class GroupsController : Controller
    {
        #region Properties
        private IGroupRepository groupRepository;
        private IActivityRepository activityRepository;
        #endregion

        #region Constructors
        public GroupsController()
        {
            this.groupRepository = new GroupRepository(new GymContext());
            this.activityRepository = new ActivityRepository(new GymContext());
        }

        public GroupsController(IGroupRepository groupRepository, IActivityRepository activityRepository)
        {
            this.groupRepository = groupRepository;
            this.activityRepository = activityRepository;
        }
        #endregion

        // GET: Groups
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

            var groups = groupRepository.GetGroups();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                groups = groups.Where(c => c.Activity.Name.ToLower().Contains(searchString));
            }
            #endregion

            #region OrderBy
            ViewBag.ActivityNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.GroupNameSortParm = sortOrder == "group_asc" ? "group_desc" : "group_asc";
            ViewBag.LevelSortParm = sortOrder == "level_asc" ? "level_desc" : "level_asc";
            ViewBag.QuotaSortParm = sortOrder == "quota_asc" ? "quota_desc" : "quota_asc";
            ViewBag.AmountSortParm = sortOrder == "amount_asc" ? "amount_desc" : "amount_asc";
            ViewBag.DaySortParm = sortOrder == "day_asc" ? "day_desc" : "day_asc";
            ViewBag.HourFromSortParm = sortOrder == "hourFrom_asc" ? "hourFrom_desc" : "hourFrom_asc";
            ViewBag.HourToSortParm = sortOrder == "hourTo_asc" ? "hourTo_desc" : "hourTo_asc";

            switch (sortOrder)
            {
                case "name_desc":
                    groups = groups.OrderByDescending(a => a.Activity.Name);
                    break;
                case "group_desc":
                    groups = groups.OrderByDescending(a => a.Name);
                    break;
                case "group_asc":
                    groups = groups.OrderBy(a => a.Name);
                    break;
                case "level_desc":
                    groups = groups.OrderByDescending(a => a.Level);
                    break;
                case "level_asc":
                    groups = groups.OrderBy(a => a.Level);
                    break;
                case "quota_desc":
                    groups = groups.OrderByDescending(a => a.Quota);
                    break;
                case "quota_asc":
                    groups = groups.OrderBy(a => a.Quota);
                    break;
                case "amount_desc":
                    groups = groups.OrderByDescending(a => a.Amount);
                    break;
                case "amount_asc":
                    groups = groups.OrderBy(a => a.Amount);
                    break;
                case "day_desc":
                    groups = groups.OrderByDescending(a => a.Day);
                    break;
                case "day_asc":
                    groups = groups.OrderBy(a => a.Day);
                    break;
                case "hourFrom_desc":
                    groups = groups.OrderByDescending(a => a.HourFrom);
                    break;
                case "hourFrom_asc":
                    groups = groups.OrderBy(a => a.HourFrom);
                    break;
                case "hourTo_desc":
                    groups = groups.OrderByDescending(a => a.HourTo);
                    break;
                case "hourTo_asc":
                    groups = groups.OrderBy(a => a.HourTo);
                    break;
                default:
                    groups = groups.OrderBy(a => a.Activity.Name);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(groups.ToPagedList(pageNumber, pageSize));
        }

        // GET: ActivitySchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = groupRepository.GetGroupByID((int)id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name");
            return View();
        }

        // POST: Groups/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GroupID,Name,Description,Level,Quota,Amount,Day,HourFrom,HourTo,ActivityID")] Group group)
        {
            if (ModelState.IsValid)
            {
                groupRepository.InsertGroup(group);
                groupRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", group.ActivityID);
            return View(group);
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = groupRepository.GetGroupByID((int)id);
            if (group == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", group.ActivityID);
            return View(group);
        }

        // POST: Groups/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GroupID,Day,Name,Description,Level,Quota,Amount,Day,HourFrom,HourTo,ActivityID")] Group group)
        {
            if (ModelState.IsValid)
            {
                groupRepository.UpdateGroup(group);
                groupRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", group.ActivityID);
            return View(group);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = groupRepository.GetGroupByID((int)id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: ActivitySchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = groupRepository.GetGroupByID((int)id);
            groupRepository.DeleteGroup((int)id);
            groupRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                groupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
