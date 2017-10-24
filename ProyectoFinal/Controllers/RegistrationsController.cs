using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Filters;
using ProyectoFinal.Models.Repositories;
using System.Configuration;
using PagedList;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin", OtherRole = "Instructor")]
    [HandleError()]
    public class RegistrationsController : Controller
    {

        #region Properties
        private IRegistrationRepository registrationRepository;
        private IClientRepository clientRepository;
        private IGroupRepository groupRepository;
        #endregion

        #region Constructors
        public RegistrationsController()
        {
            this.registrationRepository = new RegistrationRepository(new GymContext());
            this.clientRepository = new ClientRepository(new GymContext());
            this.groupRepository = new GroupRepository(new GymContext());
        }

        public RegistrationsController(IRegistrationRepository registrationRepository, IClientRepository clientRepository, IGroupRepository groupRepository)
        {
            this.registrationRepository = registrationRepository;
            this.clientRepository = clientRepository;
            this.groupRepository = groupRepository;
        }
        #endregion
        private GymContext db = new GymContext();

        //GET: Registrations
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

            var registrations = registrationRepository.GetRegistrations();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                registrations = registrations.Where(r => r.RegistrationID.ToString().ToLower().Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.CreationDateSortParm = sortOrder == "creationDate_asc" ? "creationDate_desc" : "creation_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewBag.ClientFirstNameSortParm = sortOrder == "FirstName_asc" ? "FirstName_desc" : "FirstName_asc";
            ViewBag.GroupNameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";

            switch (sortOrder)
            {
                case "creationDate_desc":
                    registrations = registrations.OrderByDescending(s => s.CreationDate);
                    break;
                case "creationDate_asc":
                    registrations = registrations.OrderBy(s => s.CreationDate);
                    break;
                case "status_asc":
                    registrations = registrations.OrderBy(s => s.Status);
                    break;
                case "article_desc":
                    registrations = registrations.OrderByDescending(s => s.Status);
                    break;
                case "FirstName_asc":
                    registrations = registrations.OrderBy(s => s.Client.FirstName);
                    break;
                case "FirstName_desc":
                    registrations = registrations.OrderByDescending(s => s.Client.FirstName);
                    break;
                case "name_asc":
                    registrations = registrations.OrderBy(s => s.Group.Name);
                    break;
                case "name_desc":
                    registrations = registrations.OrderByDescending(s => s.Group.Name);
                    break;
                default:
                    registrations = registrations.OrderBy(s => s.RegistrationID);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(registrations.ToPagedList(pageNumber, pageSize));
        }

        // GET: Registrations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = registrationRepository.GetRegistrationByID((int)id);
            if (registration == null)
            {
                return HttpNotFound();
            }
           
            return View(registration);
        }
        public ActionResult Create()
        {
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");
            ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name");
            return View();
        }

        // POST: Registration/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RegistrationID,CreationDate,Status,ClientID,GroupID")] Registration registration)
        {
            int groupID = Convert.ToInt32(Request.Params["GroupID"]);
            int clientID = Convert.ToInt32(Request.Params["ClientID"]);
           // var actividad = groupRepository.GetGroupByID(groupID);
           // int activityId = actividad.ActivityID;
            var regi = groupRepository;
            if (registrationRepository.ValidarAbonoActivo(clientID, groupID))
            {
                ModelState.AddModelError("GroupID", "El cliente no tiene un abono activo");
                ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");
                ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name");
                return View();
            }
            if (regi.AlumnoGrupo(groupID, clientID))
            {
                ModelState.AddModelError("GroupID", "El socio ya esta registrado en esa clase");
                ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");
                ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name");
                return View();
            }
            if (registrationRepository.HorarioClase(clientID, groupID))
            {
                ModelState.AddModelError("GroupID", "Horario de clase superpuesto con otra clase");
                ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");
                ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name");
                return View();
            }
            if (registrationRepository.ValidarCupo(groupID))
            {
                ModelState.AddModelError("GroupID", "Esta clase no tiene más cupo");
                ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName");
                ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name");
                return View();
            }
           
            if (ModelState.IsValid)
            {
                registration.CreationDate = DateTime.Now;
                registrationRepository.InsertRegistration(registration);
                groupRepository.AgregarInscripto(groupID);
                groupRepository.DecrementarCupo(groupID);
                groupRepository.Save();
                registrationRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", registration.ClientID);
            ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name", registration.GroupID);
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = registrationRepository.GetRegistrationByID((int)id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", registration.ClientID);
            ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name", registration.GroupID);
            return View(registration);
        }


        // POST: Registrations/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Registration registration)
        {
            if (ModelState.IsValid)
            {
                registrationRepository.UpdateRegistration(registration);
                registrationRepository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(clientRepository.GetClients(), "ClientID", "FirstName", registration.ClientID);
            ViewBag.GroupID = new SelectList(groupRepository.GetGroups(), "GroupID", "Name", registration.GroupID);
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = registrationRepository.GetRegistrationByID((int)id);
            if (registration == null)
            {
                return HttpNotFound();
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Registration registration = registrationRepository.GetRegistrationByID((int)id);
            var groupID = registration.GroupID;
            groupRepository.EliminarInscripto(groupID);
            groupRepository.IncrementarCupo(groupID);
            registrationRepository.DeleteRegistration((int)id);
            registrationRepository.Save();
            groupRepository.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                registrationRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}