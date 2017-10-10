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

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Client")]
    public class RegistrationClientsController : Controller
    {
        private GymContext db = new GymContext();

        #region Properties
        private IClientRepository clientRepository;
        private IGroupRepository groupRepository;
        #endregion

        #region Constructors
        public RegistrationClientsController()
        {
            this.clientRepository = new ClientRepository(new GymContext());
            this.groupRepository = new GroupRepository(new GymContext());
        }

        public RegistrationClientsController(IClientRepository clientRepository, IGroupRepository groupRepository)
        {
            this.clientRepository = clientRepository;
            this.groupRepository = groupRepository;
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
            registration.Status = "Active";
            
            registration.GroupID = Convert.ToInt32(Request.Params["GroupID"]);
            //registration.GroupID = (ViewBag.Group as Group).GroupID;
            registration.ClientID = this.GetLoggedUser().ClientID;

            if (ModelState.IsValid)
            {
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
            db.Registrations.Remove(registration);
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
