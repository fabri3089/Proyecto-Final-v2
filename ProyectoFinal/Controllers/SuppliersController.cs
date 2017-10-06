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
    public class SuppliersController : Controller
    {
        #region Properties
        private ISupplierRepository supplierRepository;
        #endregion

        #region Constructors
        public SuppliersController(ISupplierRepository supplierRepository)
        {
            this.supplierRepository = supplierRepository;
        }

        public SuppliersController()
        {
            this.supplierRepository = new SupplierRepository(new GymContext());
        }
        #endregion

        // GET: Suppliers
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

            var suppliers = supplierRepository.GetSuppliers();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                suppliers = suppliers.Where(r => r.BusinessName.ToLower().Contains(searchString.ToString()));
            }
            #endregion

            #region OrderBy
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewBag.TelSortParm = sortOrder == "tel_asc" ? "tel_desc" : "tel_asc";
            ViewBag.AddressSortParm = sortOrder == "address_asc" ? "address_desc" : "address_asc";
            ViewBag.CitySortParm = sortOrder == "city_asc" ? "city_desc" : "city_asc";
            ViewBag.WebSortParm = sortOrder == "web_asc" ? "web_desc" : "web_asc";

            switch (sortOrder)
            {
                case "name_desc":
                    suppliers = suppliers.OrderByDescending(s => s.BusinessName);
                    break;
                case "email_desc":
                    suppliers = suppliers.OrderBy(s => s.Email);
                    break;
                case "email_asc":
                    suppliers = suppliers.OrderByDescending(s => s.Email);
                    break;
                case "tel_desc":
                    suppliers = suppliers.OrderByDescending(s => s.PhoneNumber);
                    break;
                case "tel_asc":
                    suppliers = suppliers.OrderBy(s => s.PhoneNumber);
                    break;
                case "address_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Address);
                    break;
                case "address_asc":
                    suppliers = suppliers.OrderBy(s => s.Address);
                    break;
                case "city_desc":
                    suppliers = suppliers.OrderByDescending(s => s.City);
                    break;
                case "city_asc":
                    suppliers = suppliers.OrderBy(s => s.City);
                    break;
                case "web_desc":
                    suppliers = suppliers.OrderByDescending(s => s.WebSite);
                    break;
                case "web_asc":
                    suppliers = suppliers.OrderBy(s => s.WebSite);
                    break;
                default:
                    suppliers = suppliers.OrderBy(s => s.BusinessName);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(suppliers.ToPagedList(pageNumber, pageSize));
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierRepository.GetSupplierByID((int)id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplierRepository.InsertSupplier(supplier);
                supplierRepository.Save();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierRepository.GetSupplierByID((int)id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                supplierRepository.UpdateSupplier(supplier);
                supplierRepository.Save();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = supplierRepository.GetSupplierByID((int)id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = supplierRepository.GetSupplierByID((int)id);
            supplierRepository.DeleteSupplier((int)id);
            supplierRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                supplierRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
