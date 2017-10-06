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
    public class StocksController : Controller
    {
        #region Properties
        private IStockRepository stockRepository;
        #endregion

        #region Constructors
        public StocksController()
        {
            this.stockRepository = new StockRepository(new GymContext());
        }

        public StocksController(IStockRepository stockRepository)
        {
            this.stockRepository = stockRepository;
        }
        #endregion

        // GET: Stocks
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

            var stocks = stockRepository.GetStocks();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                stocks = stocks.Where(r => r.ArticleID.ToString().ToLower().Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.ArticleSortParm = sortOrder == "article_asc" ? "article_desc" : "article_asc";
            ViewBag.StockSortParm = String.IsNullOrEmpty(sortOrder) ? "stock_desc" : "";
            ViewBag.DesiredStockSortParm = sortOrder == "desired_asc" ? "desired_desc" : "desired_asc";

            switch (sortOrder)
            {
                case "stock_desc":
                    stocks = stocks.OrderByDescending(s => s.CantInStock);
                    break;
                case "article_desc":
                    stocks = stocks.OrderBy(s => s.ArticleID);
                    break;
                case "article_asc":
                    stocks = stocks.OrderByDescending(s => s.ArticleID);
                    break;
                case "desired_desc":
                    stocks = stocks.OrderByDescending(s => s.DesiredStock);
                    break;
                case "desired_asc":
                    stocks = stocks.OrderBy(s => s.DesiredStock);
                    break;
                default:
                    stocks = stocks.OrderBy(s => s.CantInStock);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(stocks.ToPagedList(pageNumber, pageSize));
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = stockRepository.GetStockByID((int)id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StockID,ArticleID,CantInStock,DesiredStock")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                stockRepository.InsertStock(stock);
                stockRepository.Save();
                return RedirectToAction("Index");
            }

            return View(stock);
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = stockRepository.GetStockByID((int)id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StockID,ArticleID,CantInStock,DesiredStock")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                stockRepository.UpdateStock(stock);
                stockRepository.Save();
                return RedirectToAction("Index");
            }
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = stockRepository.GetStockByID((int)id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = stockRepository.GetStockByID((int)id);
            stockRepository.DeleteStock((int)id);
            stockRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                stockRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
