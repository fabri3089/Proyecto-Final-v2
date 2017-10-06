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

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin")]
    [HandleError()]
    public class AssistancesController : Controller
    {
        #region Properties
        private IAssistanceRepository assistanceRepository;
        #endregion

        #region Constructors
        public AssistancesController()
        {
            this.assistanceRepository = new AssistanceRepository(new GymContext());
        }

        public AssistancesController(IAssistanceRepository assistanceRepository)
        {
            this.assistanceRepository = assistanceRepository;
        }
        #endregion

        // GET: Assistances
        public ActionResult Index(int? page)
        {
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 10;
            var assistances = assistanceRepository.GetAssistances();
                                                  //.AsPagination(page ?? 1, pageSize);
            return View(assistances);
        }

        // GET: Assistances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assistance assistance = assistanceRepository.GetAssistanceByID((int)id);
            if (assistance == null)
            {
                return HttpNotFound();
            }
            return View(assistance);
        }

        // GET: Assistances/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Assistances/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssistanceID,assistanceDate,ClientID")] Assistance assistance)
        {
            if (ModelState.IsValid)
            {
                assistanceRepository.InsertAssistance(assistance);
                assistanceRepository.Save();
                return RedirectToAction("Index");
            }

            return View(assistance);
        }

        // GET: Assistances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assistance assistance = assistanceRepository.GetAssistanceByID((int)id);
            if (assistance == null)
            {
                return HttpNotFound();
            }
            return View(assistance);
        }

        // POST: Assistances/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssistanceID,assistanceDate,ClientID")] Assistance assistance)
        {
            if (ModelState.IsValid)
            {
                assistanceRepository.UpdateAssistance(assistance);
                assistanceRepository.Save();
                return RedirectToAction("Index");
            }
            return View(assistance);
        }

        // GET: Assistances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assistance assistance = assistanceRepository.GetAssistanceByID((int)id);
            if (assistance == null)
            {
                return HttpNotFound();
            }
            return View(assistance);
        }

        // POST: Assistances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assistance assistance = assistanceRepository.GetAssistanceByID((int)id);
            assistanceRepository.DeleteAssistance(id);
            assistanceRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                assistanceRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
