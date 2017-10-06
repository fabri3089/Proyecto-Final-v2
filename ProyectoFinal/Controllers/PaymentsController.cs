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
    public class PaymentsController : Controller
    {
        #region Properties
        private IPaymentRepository paymentRepository;
        private IPaymentTypeRepository paymentTypeRepository;
        private IClientRepository clientRepository;
        #endregion

        #region Constructors
        public PaymentsController()
        {
            this.paymentRepository = new PaymentRepository(new GymContext());
            this.paymentTypeRepository = new PaymentTypeRepository(new GymContext());
            this.clientRepository = new ClientRepository(new GymContext());
        }

        public PaymentsController(IPaymentRepository paymentRepository, IPaymentTypeRepository paymentTypeRepository, IClientRepository clientRepository)
        {
            this.paymentRepository = paymentRepository;
            this.paymentTypeRepository = paymentTypeRepository;
            this.clientRepository = clientRepository;
        }
        #endregion


        // GET: Payments
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

            var payments = paymentRepository.GetPayments();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                payments = payments.Where(p => p.Client.FirstName.ToLower().Contains(searchString.ToLower()) || p.Client.LastName.ToLower().Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "surname_asc" ? "surname_desc" : "surname_asc";
            ViewBag.DescriptionSortParm = sortOrder == "description_asc" ? "description_desc" : "description_asc";
            ViewBag.ExpDateSortParm = sortOrder == "expDate_asc" ? "expDate_desc" : "expDate_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";

            switch (sortOrder)
            {
                case "name_desc":
                    payments = payments.OrderByDescending(p => p.Client.FirstName);
                    break;
                case "surname_desc":
                    payments = payments.OrderByDescending(p => p.Client.LastName);
                    break;
                case "surname_asc":
                    payments = payments.OrderBy(p => p.Client.LastName);
                    break;
                case "description_desc":
                    payments = payments.OrderByDescending(p => p.PaymentType.Description);
                    break;
                case "description_asc":
                    payments = payments.OrderBy(p => p.PaymentType.Description);
                    break;
                case "status_desc":
                    payments = payments.OrderByDescending(p => p.Status);
                    break;
                case "status_asc":
                    payments = payments.OrderBy(p => p.Status);
                    break;
                case "expDate_desc":
                    payments = payments.OrderByDescending(p => p.ExpirationDate);
                    break;
                case "expDate_asc":
                    payments = payments.OrderBy(p => p.ExpirationDate);
                    break;
                default:
                    payments = payments.OrderBy(p => p.Client.FirstName);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(payments.ToPagedList(pageNumber, pageSize));


        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = paymentRepository.GetPaymentByID((int)id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");
            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description");
            return View();
        }

        // POST: Payments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentID,Status,ClientID,PaymentTypeID")] Payment payment)
        {
            #region Seteo fecha creacion, fecha expiracion de abono y Status
            var paymentType = paymentTypeRepository.GetPaymentTypeByID(payment.PaymentTypeID);
            payment.CreationDate = DateTime.Now;
            payment.ExpirationDate = DateTime.Now.AddMonths(paymentType.DurationInMonths);
            payment.Status = Utils.Catalog.Status.Active;
            #endregion

            if (ModelState.IsValid)
            {
                paymentRepository.InsertPayment(payment);
                paymentRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", payment.ClientID);
            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description", payment.PaymentTypeID);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = paymentRepository.GetPaymentByID((int)id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", payment.ClientID);
            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description", payment.PaymentTypeID);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                paymentRepository.UpdatePayment(payment);
                paymentRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", payment.ClientID);
            ViewBag.PaymentTypeID = new SelectList(paymentTypeRepository.GetPaymentTypes(), "PaymentTypeID", "Description", payment.PaymentTypeID);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = paymentRepository.GetPaymentByID((int)id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = paymentRepository.GetPaymentByID((int)id);
            paymentRepository.DeletePayment((int)id);
            paymentRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                paymentRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
