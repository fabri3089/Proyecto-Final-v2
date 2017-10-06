using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinal.Models;
using ProyectoFinal.Utils;
using ProyectoFinal.Models.Repositories;
using System.Configuration;
using ProyectoFinal.Filters;
using System.Diagnostics;
using PagedList;
using API.Services;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin", OtherRole = "Instructor")]
    [HandleError()]
    public class ClientsController : Controller
    {
        #region Properties
        private IClientRepository clientRepository;
        #endregion

        #region Constructors
        public ClientsController()
        {
            this.clientRepository = new ClientRepository(new GymContext());
        }

        public ClientsController(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }
        #endregion

        #region ActionMethods
        // GET: Clients
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

            var clients = clientRepository.GetClients();

            #region search

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(r => string.Concat(r.FirstName, " ", r.LastName)
                                                   .ToLower()
                                                   .Contains(searchString.ToLower()));
            }
            #endregion

            #region OrderBy
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.SurnameSortParm = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";
            ViewBag.DocTypeSortParm = sortOrder == "docType_desc" ? "docType_asc" : "docType_desc";
            ViewBag.DocNumberSortParm = sortOrder == "doc_asc" ? "doc_desc" : "doc_asc";
            ViewBag.BirthDateSortParm = sortOrder == "birth_asc" ? "birth_desc" : "birth_asc";
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewBag.RolSortParm = sortOrder == "rol_asc" ? "rol_desc" : "rol_asc";

            switch(sortOrder)
            {
                case "name_desc":
                    clients = clients.OrderByDescending(c => c.FirstName);
                    break;
                case "name_asc":
                    clients = clients.OrderBy(c => c.FirstName);
                    break;
                case "surname_desc":
                    clients = clients.OrderByDescending(c => c.LastName);
                    break;
                case "birth_desc":
                    clients = clients.OrderByDescending(c => c.BirthDate);
                    break;
                case "birth_asc":
                    clients = clients.OrderBy(c => c.BirthDate);
                    break;
                case "docType_desc":
                    clients = clients.OrderByDescending(c => c.DocType);
                    break;
                case "docType_asc":
                    clients = clients.OrderBy(c => c.DocType);
                    break;
                case "doc_desc":
                    clients = clients.OrderByDescending(c => c.DocNumber);
                    break;
                case "doc_asc":
                    clients = clients.OrderBy(c => c.DocNumber);
                    break;
                case "email_desc":
                    clients = clients.OrderByDescending(c => c.Email);
                    break;
                case "email_asc":
                    clients = clients.OrderBy(c => c.Email);
                    break;
                case "rol_desc":
                    clients = clients.OrderByDescending(c => c.Role);
                    break;
                case "rol_asc":
                    clients = clients.OrderBy(c => c.Role);
                    break;
                default:
                    clients = clients.OrderBy(c => c.LastName);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            int pageSize = ConfigurationManager.AppSettings["PageSize"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]) : 8;
            return View(clients.ToPagedList(pageNumber, pageSize));
        }

        // GET: Clients/Details/5
        public ActionResult Details(int id)
        {
            Client client = clientRepository.GetClientByID(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            return View(client);

        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Client client)
        {
            if (clientRepository.IsEmailAlreadyInUse(client))
            {
                ModelState.AddModelError("Email", "El email ya está en uso");
            }
            else if (ModelState.IsValid)
            {
                var password = client.Password;
                clientRepository.HashPassword(client);
                clientRepository.InsertClient(client);
                clientRepository.Save();

                SendGridMailing sg = new SendGridMailing();
                var templatePath = Server.MapPath(@"~/Templates/EmailBienvenida.html");

                sg.Execute(templatePath, client.Email, password);

                return RedirectToAction("Index", new { sortOrder = string.Empty, searchString = string.Empty });
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = clientRepository.GetClientByID((int)id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if(clientRepository.IsEmailAlreadyInUse(client))
            {
                ModelState.AddModelError("Email", "El email ya está en uso");
            }
            else if (ModelState.IsValid)
            {
                clientRepository.UpdateClient(client);
                clientRepository.Save();
                return RedirectToAction("Index", new { sortOrder = string.Empty, searchString = string.Empty });
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = clientRepository.GetClientByID((int)id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = clientRepository.GetClientByID(id);
            clientRepository.DeleteClient(id);
            clientRepository.Save();
            return RedirectToAction("Index", new { sortOrder = string.Empty, searchString = string.Empty });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                clientRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
