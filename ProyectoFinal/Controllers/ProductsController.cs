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
using ProyectoFinal.Filters;
using PagedList;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin")]
    [HandleError()]
    public class ProductsController : Controller
    {
        #region Properties
        private IProductRepository productRepository;
        private ISupplierRepository supplierRepository;
        #endregion

        #region Constructors
        public ProductsController()
        {
            this.productRepository = new ProductRepository(new GymContext());
            this.supplierRepository = new SupplierRepository(new GymContext());
        }

        public ProductsController(IProductRepository ProductRepository, ISupplierRepository supplierRepository)
        {
            
            this.productRepository = ProductRepository;
            this.supplierRepository = supplierRepository;
        }
        #endregion

        // GET: Products
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

            var products = productRepository.GetProducts();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(c => c.Name.ToLower().Contains(searchString.ToLower()) || c.Type.ToString().ToLower().Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.TypeSortParm = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.PriceSortParm = sortOrder == "price_asc" ? "price_desc" : "price_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewBag.SupplierSortParm = sortOrder == "supplier_asc" ? "supplier_desc" : "supplier_asc";
            ViewBag.PurchaseSortParm = sortOrder == "purchase_asc" ? "purchase_desc" : "purchase_asc";

            switch (sortOrder)
            {
                case "type_desc":
                    products = products.OrderByDescending(p => p.Type.ToString());
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "status_desc":
                    products = products.OrderByDescending(p => p.Status.ToString());
                    break;
                case "status_asc":
                    products = products.OrderBy(p => p.Status.ToString());
                    break;
                case "supplier_desc":
                    products = products.OrderByDescending(p => p.Supplier.BusinessName);
                    break;
                case "supplier_asc":
                    products = products.OrderBy(p => p.Supplier.BusinessName);
                    break;
                case "purchase_desc":
                    products = products.OrderByDescending(p => p.PurchaseDate);
                    break;
                case "purchase_asc":
                    products = products.OrderBy(p => p.PurchaseDate);
                    break;
                default:
                    products = products.OrderBy(p => p.Type.ToString());
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = productRepository.GetProductByID((int)id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return View(Product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(supplierRepository.GetSuppliers(), "SupplierID", "BusinessName");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product Product)
        {
            if (ModelState.IsValid)
            {
                productRepository.InsertProduct(Product);
                productRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierID = new SelectList(supplierRepository.GetSuppliers(), "SupplierID", "BusinessName", Product.SupplierID);
            return View(Product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = productRepository.GetProductByID((int)id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(supplierRepository.GetSuppliers(), "SupplierID", "BusinessName", Product.SupplierID);
            return View(Product);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product Product)
        {
            if (ModelState.IsValid)
            {
                productRepository.UpdateProduct(Product);
                productRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierID = new SelectList(supplierRepository.GetSuppliers(), "SupplierID", "BusinessName", Product.SupplierID);
            return View(Product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product Product = productRepository.GetProductByID((int)id);
            if (Product == null)
            {
                return HttpNotFound();
            }
            return View(Product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product Product = productRepository.GetProductByID((int)id);
            productRepository.DeleteProduct((int)id);
            productRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                productRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
