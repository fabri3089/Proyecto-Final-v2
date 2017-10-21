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
using ProyectoFinal.Utils;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Client")]
    public class RegistrationClientsController : Controller
    {
        private GymContext db = new GymContext();

        #region Properties
        private IClientRepository clientRepository;
        private IGroupRepository groupRepository;
        private IRegistrationRepository registrationRepository;
        #endregion

        #region Constructors
        public RegistrationClientsController()
        {
            this.clientRepository = new ClientRepository(new GymContext());
            this.groupRepository = new GroupRepository(new GymContext());
            this.registrationRepository = new RegistrationRepository(new GymContext());
        }

        public RegistrationClientsController(IClientRepository clientRepository, IGroupRepository groupRepository, IRegistrationRepository registrationRepository)
        {
            this.clientRepository = clientRepository;
            this.groupRepository = groupRepository;
            this.registrationRepository = registrationRepository;
        }
        #endregion

        private Client GetLoggedUser()
        {
            try
            {
                var loggedUser = Session["User"] as Client;
                var currentUser = clientRepository.GetClientByID(loggedUser.ClientID);

                if (currentUser != null)
                    return currentUser;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        // GET: RegistrationClients
        public ActionResult Index()
        {
            var loggedUserID = this.GetLoggedUser().ClientID;

            var registrations = groupRepository.GetGroupsAvailable(loggedUserID);

            //var registrations = db.Registrations.Include(r => r.Client).Include(r => r.Group)
              //                                  .Where(r => r.ClientID != loggedUserID);

            var clientRegistrations = db.Registrations
                                            .Include(r => r.Client)
                                            .Include(r => r.Group)
                                            .Where(r => r.ClientID == loggedUserID);


            ViewBag.ClientRegistrations = clientRegistrations.ToList();

            return View(registrations.ToList());
        }

        // GET: RegistrationClients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: RegistrationClients/Create
        public ActionResult Create(int id)
        {
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "FirstName");
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Name");

            ViewBag.Client = this.GetLoggedUser();
            var group = groupRepository.GetGroupByID(id);
            int groupID = group.GroupID;
            ViewBag.Group = group;
            return View();
        }

        // POST: RegistrationClients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Registration registration)
        {
            registration.CreationDate = DateTime.Now;
            registration.GroupID = Convert.ToInt32(Request.Params["GroupID"]);
            int groupID = registration.GroupID;
            registration.ClientID = this.GetLoggedUser().ClientID;
            int clientID = registration.ClientID;
            registration.Status = Catalog.Status.Active;
            if(registrationRepository.HorarioClase(clientID, groupID))
            {
                ModelState.AddModelError("GroupID", "Horario de clase superpuesto con otra clase");
                ViewBag.Client = this.GetLoggedUser();
                var group = groupRepository.GetGroupByID(groupID);
                ViewBag.Group = group;
                return View();
            }
            if (ModelState.IsValid)
            {
                groupRepository.AgregarAlumno(groupID);
                groupRepository.Save();
                db.Registrations.Add(registration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "FirstName", registration.ClientID);
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Name", registration.GroupID);
            return View(registration);
        }

        // GET: RegistrationClients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "FirstName", registration.ClientID);
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Name", registration.GroupID);
            return View(registration);
        }

        // POST: RegistrationClients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RegistrationID,CreationDate,Status,ClientID,GroupID")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "FirstName", registration.ClientID);
            ViewBag.GroupID = new SelectList(db.Groups, "GroupID", "Name", registration.GroupID);
            return View(registration);
        }

        // GET: RegistrationClients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registration registration = db.Registrations.Find(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            
            return View(registration);
        }

        // POST: RegistrationClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Registration registration = db.Registrations.Find(id);
            var groupID = registration.GroupID;
            db.Registrations.Remove(registration);
            groupRepository.EliminarInscripto(groupID);
            groupRepository.Save();
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
