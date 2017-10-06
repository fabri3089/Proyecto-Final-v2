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
    [AuthorizationPrivilege(Role = "Admin")]
    [HandleError()]
    public class PaymentTypePricesController : Controller
    {
        #region Properties
        private IPaymentTypePriceRepository paymentTypePriceRepository;
        private IPaymentTypeRepository paymentTypeRepository;
        #endregion

        #region Constructors
        public PaymentTypePricesController()
        {
            this.paymentTypePriceRepository = new PaymentTypePriceRepository(new GymContext());
            this.paymentTypeRepository = new PaymentTypeRepository(new GymContext());
        }

        public PaymentTypePricesController(IPaymentTypePriceRepository paymentTypePriceRepository, IPaymentTypeRepository paymentTypeRepository)
        {
            this.paymentTypePriceRepository = paymentTypePriceRepository;
            this.paymentTypeRepository = paymentTypeRepository;
        }
        #endregion


        // GET: PaymentTypePrices
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

            var paymentTypePrices = paymentTypePriceRepository.GetPaymentTypePrices();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                paymentTypePrices = paymentTypePrices.Where(p => p.PaymentType.Description.ToLower().Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.DescripionSortParm = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.DateFromSortParm = sortOrder == "dateFrom_asc" ? "dateFrom_desc" : "dateFrom_asc";

            switch (sortOrder)
            {
                case "description_desc":
                    paymentTypePrices = paymentTypePrices.OrderByDescending(p => p.PaymentType.Description);
                    break;
                case "price_desc":
                    paymentTypePrices = paymentTypePrices.OrderByDescending(p => p.Price);
                    break;
                case "price_asc":
                    paymentTypePrices = paymentTypePrices.OrderBy(p => p.Price);
                    break;
                case "dateFrom_desc":
                    paymentTypePrices = paymentTypePrices.OrderByDescending(p => p.DateFrom);
                    break;
                case "dateFrom_asc":
                    paymentTypePrices = paymentTypePrices.OrderBy(p => p.DateFrom);
                    break;
                default:
                    paymentTypePrices = paymentTypePrices.OrderBy(p => p.PaymentType.Description);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(paymentTypePrices.ToPagedList(pageNumber, pageSize));
        }

        // GET: PaymentTypePrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentTypePrice paymentTypePrice = paymentTypePriceRepository.GetPaymentTypePriceByID((int)id);
            if (paymentTypePrice == null)
            {
                return HttpNotFound();
            }
            return View(paymentTypePrice);
        }

        // GET: PaymentTypePrices/Create
        public ActionResult Create()
        {
            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description");
            return View();
        }

        // POST: PaymentTypePrices/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentTypePriceID,Price,DateFrom,PaymentTypeID")] PaymentTypePrice paymentTypePrice)
        {
            if (ModelState.IsValid)
            {
                paymentTypePriceRepository.InsertPaymentTypePrice(paymentTypePrice);
                paymentTypePriceRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description", paymentTypePrice.PaymentTypeID);
            return View(paymentTypePrice);
        }

        // GET: PaymentTypePrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentTypePrice paymentTypePrice = paymentTypePriceRepository.GetPaymentTypePriceByID((int)id);
            if (paymentTypePrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description", paymentTypePrice.PaymentTypeID);
            return View(paymentTypePrice);
        }

        // POST: PaymentTypePrices/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaymentTypePriceID,Price,DateFrom,PaymentTypeID")] PaymentTypePrice paymentTypePrice)
        {
            if (ModelState.IsValid)
            {
                paymentTypePriceRepository.UpdatePaymentTypePrice(paymentTypePrice);
                paymentTypePriceRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description", paymentTypePrice.PaymentTypeID);
            return View(paymentTypePrice);
        }

        // GET: PaymentTypePrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentTypePrice paymentTypePrice = paymentTypePriceRepository.GetPaymentTypePriceByID((int)id);
            if (paymentTypePrice == null)
            {
                return HttpNotFound();
            }
            return View(paymentTypePrice);
        }

        // POST: PaymentTypePrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentTypePrice paymentTypePrice = paymentTypePriceRepository.GetPaymentTypePriceByID((int)id);
            paymentTypePriceRepository.DeletePaymentTypePrice((int)id);
            paymentTypePriceRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                paymentTypePriceRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
