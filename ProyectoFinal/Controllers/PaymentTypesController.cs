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
    public class PaymentTypesController : Controller
    {
        #region Properties
        private IPaymentTypeRepository paymentTypeRepository;
        private IActivityRepository activityRepository;
        #endregion

        #region Constructors
        public PaymentTypesController()
        {
            this.paymentTypeRepository = new PaymentTypeRepository(new GymContext());
            this.activityRepository = new ActivityRepository(new GymContext());
        }

        public PaymentTypesController(IPaymentTypeRepository paymentTypeRepository, IActivityRepository activityRepository)
        {
            this.paymentTypeRepository = paymentTypeRepository;
            this.activityRepository = activityRepository;
        }
        #endregion

        // GET: PaymentTypes
        [AuthorizationPrivilege(Role = "Admin")]
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

            var paymentTypes = paymentTypeRepository.GetPaymentTypes();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                paymentTypes = paymentTypes.Where(p => p.Activity.Name.ToLower().Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.ActivitySortParm = String.IsNullOrEmpty(sortOrder) ? "activity_desc" : "";
            ViewBag.DescriptionSortParm = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewBag.MonthsSortParm = sortOrder == "months_asc" ? "months_desc" : "months_asc";

            switch (sortOrder)
            {
                case "activity_desc":
                    paymentTypes = paymentTypes.OrderByDescending(p => p.Activity.Name);
                    break;
                case "description_desc":
                    paymentTypes = paymentTypes.OrderByDescending(p => p.Description);
                    break;
                case "description_asc":
                    paymentTypes = paymentTypes.OrderBy(p => p.Description);
                    break;
                case "status_desc":
                    paymentTypes = paymentTypes.OrderByDescending(p => p.Status);
                    break;
                case "status_asc":
                    paymentTypes = paymentTypes.OrderByDescending(p => p.Status);
                    break;
                case "months_desc":
                    paymentTypes = paymentTypes.OrderBy(p => p.DurationInMonths);
                    break;
                case "months_asc":
                    paymentTypes = paymentTypes.OrderByDescending(p => p.DurationInMonths);
                    break;
                default:
                    paymentTypes = paymentTypes.OrderBy(p => p.Activity.Name);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(paymentTypes.ToPagedList(pageNumber, pageSize));
        }

        // GET: PaymentTypes/Details/5
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = paymentTypeRepository.GetPaymentTypeByID((int)id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // GET: PaymentTypes/Create
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult Create()
        {
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name");
            return View();
        }

        // POST: PaymentTypes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult Create([Bind(Include = "PaymentTypeID,Description,DurationInMonths,Status,ActivityID")] PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                paymentTypeRepository.InsertPaymentType(paymentType);
                paymentTypeRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", paymentType.ActivityID);
            return View(paymentType);
        }

        // GET: PaymentTypes/Edit/5
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = paymentTypeRepository.GetPaymentTypeByID((int)id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", paymentType.ActivityID);
            return View(paymentType);
        }

        // POST: PaymentTypes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult Edit([Bind(Include = "PaymentTypeID,Description,DurationInMonths,Status,ActivityID")] PaymentType paymentType)
        {
            if (ModelState.IsValid)
            {
                paymentTypeRepository.UpdatePaymentType(paymentType);
                paymentTypeRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ActivityID = new SelectList(activityRepository.GetActivities(), "ActivityID", "Name", paymentType.ActivityID);
            return View(paymentType);
        }

        public ViewResult Catalog()
        {
            var paymentTypes = paymentTypeRepository.GetPaymentTypes().OrderBy(p => p.PaymentTypeID);
            paymentTypes.ToList().RemoveAll(p => p.Status.Equals(Utils.Catalog.Status.Inactive));

            HashSet<Activity> activities = new HashSet<Activity>();
            foreach (var item in paymentTypes)
            {
                activities.Add(item.Activity);
            }
            ViewBag.Activities = activities;

            return View(paymentTypes);
        }

        // GET: PaymentTypes/Delete/5
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentType paymentType = paymentTypeRepository.GetPaymentTypeByID((int)id);
            if (paymentType == null)
            {
                return HttpNotFound();
            }
            return View(paymentType);
        }

        // POST: PaymentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizationPrivilege(Role = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentType paymentType = paymentTypeRepository.GetPaymentTypeByID((int)id);
            paymentTypeRepository.DeletePaymentType((int)id);
            paymentTypeRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                paymentTypeRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
