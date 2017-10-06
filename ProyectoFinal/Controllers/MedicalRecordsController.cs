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
    [AuthorizationPrivilege(Role = "Admin")]
    [HandleError()]
    public class MedicalRecordsController : Controller
    {
        #region Properties
        private IMedicalRecordRepository medicalRepository;
        private IClientRepository clientRepository;
        #endregion

        #region Constructors
        public MedicalRecordsController()
        {
            this.medicalRepository = new MedicalRecordRepository(new GymContext());
            this.clientRepository = new ClientRepository(new GymContext());
        }

        public MedicalRecordsController(IMedicalRecordRepository medicalRepository, IClientRepository clientRepository)
        {
            this.medicalRepository = medicalRepository;
            this.clientRepository = clientRepository;
        }
        #endregion

        // GET: MedicalRecords
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

            var medicalRecords = medicalRepository.GetMedicalRecords();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                medicalRecords = medicalRecords.Where(c => c.Client.FirstName.ToLower().Contains(searchString) || c.Client.LastName.ToLower().Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.WeigthSortParm = sortOrder == "weight_asc" ? "weight_desc" : "weight_asc";
            ViewBag.HeigthSortParm = sortOrder == "heigth_asc" ? "heigth_desc" : "heigth_asc";
            ViewBag.AgeSortParm = sortOrder == "age_asc" ? "age_desc" : "age_asc";

            switch (sortOrder)
            {
                case "name_desc":
                    medicalRecords = medicalRecords.OrderByDescending(c => c.Client.FirstName).ThenBy(c => c.Client.LastName);
                    break;
                case "weight_desc":
                    medicalRecords = medicalRecords.OrderByDescending(c => c.Weight);
                    break;
                case "weight_asc":
                    medicalRecords = medicalRecords.OrderBy(c => c.Weight);
                    break;
                case "heigth_desc":
                    medicalRecords = medicalRecords.OrderByDescending(c => c.Heigth);
                    break;
                case "heigth_asc":
                    medicalRecords = medicalRecords.OrderBy(c => c.Heigth);
                    break;
                case "age_desc":
                    medicalRecords = medicalRecords.OrderByDescending(c => c.Age);
                    break;
                case "age_asc":
                    medicalRecords = medicalRecords.OrderBy(c => c.Age);
                    break;
                default:
                    medicalRecords = medicalRecords.OrderBy(c => c.Client.FirstName).ThenBy(c => c.Client.LastName);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(medicalRecords.ToPagedList(pageNumber, pageSize));
        }

        // GET: MedicalRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = medicalRepository.GetMedicalRecordByID((int)id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Create
        public ActionResult Create(int? id)
        {
            SelectList selectList;
            if (id != null)
                selectList = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", id.ToString());
            else
                selectList = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");

            ViewBag.ClientID = selectList;
            return View();
        }

        // POST: MedicalRecords/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicalRecordID,Weight,Heigth,Age,ClientID")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                medicalRepository.InsertMedicalRecord(medicalRecord);
                medicalRepository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", medicalRecord.ClientID);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = medicalRepository.GetMedicalRecordByID((int)id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", medicalRecord.ClientID);
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicalRecordID,Weight,Heigth,Age,ClientID")] MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                medicalRepository.UpdateMedicalRecord(medicalRecord);
                medicalRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", medicalRecord.ClientID);
            return View(medicalRecord);
        }

        // GET: MedicalRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalRecord medicalRecord = medicalRepository.GetMedicalRecordByID((int)id);
            if (medicalRecord == null)
            {
                return HttpNotFound();
            }
            return View(medicalRecord);
        }

        // POST: MedicalRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalRecord medicalRecord = medicalRepository.GetMedicalRecordByID((int)id);
            medicalRepository.DeleteMedicalRecord((int)id);
            medicalRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                medicalRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
