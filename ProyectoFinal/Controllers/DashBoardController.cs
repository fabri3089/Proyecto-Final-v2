using ProyectoFinal.Filters;
using ProyectoFinal.Models;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Models.ViewModels;
using ProyectoFinal.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    [HandleError()]
    public class DashBoardController : Controller
    {
        #region Properties
        private IClientRepository clientRepository;
        private IMedicalRecordRepository medicalRecordRepository;
        #endregion

        #region Constructors
        public DashBoardController()
        {
            this.clientRepository = new ClientRepository(new GymContext());
            this.medicalRecordRepository = new MedicalRecordRepository(new GymContext());
        }

        public DashBoardController(IClientRepository clientRepository, IMedicalRecordRepository medicalRecordRepository)
        {
            this.clientRepository = clientRepository;
            this.medicalRecordRepository = medicalRecordRepository;
        }
        #endregion

        public ActionResult Index()
        {
            try
            {
                Client currentUser = this.GetLoggedUser();
                ViewBag.UserRol = currentUser.Role.Equals(Catalog.Roles.Client) ? "Socio" : (currentUser.Role.Equals(Catalog.Roles.Admin) ? "Administrador" : "Profesor");
                if (currentUser != null)
                {
                    return View("Index", currentUser);
                }
                else
                {
                    return RedirectToAction("Index", "Error", null);
                }
            }
            catch
            {
                return RedirectToAction("Index", "Error", null);
            }
        }

        // GET: Dashboard/Edit
        [HttpGet]
        public ActionResult Edit()
        {
            Client currentUser = this.GetLoggedUser();

            if (currentUser != null)
            {
                return View(currentUser);
            }
            else
            {
                return RedirectToAction("Index", "Error", null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client client)
        {
            if (clientRepository.IsEmailAlreadyInUse(client))
            {
                ModelState.AddModelError("Email", "El email ya está en uso");
            }
            else if (ModelState.IsValid)
            {
                clientRepository.UpdateClient(client);
                clientRepository.Save();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Dashboard/Edit
        [HttpGet]
        public ActionResult EditPassword()
        {
            Client currentUser = this.GetLoggedUser();
            if (currentUser != null)
            {
                ChangePassViewModel model = new ChangePassViewModel();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Error", null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPassword(ChangePassViewModel model)
        {
            Client currentUser = this.GetLoggedUser();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Por favor, complete todos campos");
            }
            else if (!model.Password.Equals(model.PasswordCheck))
            {
                ModelState.AddModelError("PasswordCheck", "Las contraseñas no coinciden");
            }
            else if(!PasswordUtilities.Compare(model.OldPassword, currentUser.Password, currentUser.PasswordSalt))
            {
                ModelState.AddModelError("OldPassword", "La contraseña actual es incorrecta");
            }
            else  //ModelState.IsValid && model.Password.Equals(model.PasswordCheck) both true
            {
                currentUser.Password = model.Password;
                clientRepository.HashPassword(currentUser);
                clientRepository.UpdateClient(currentUser);
                clientRepository.Save();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public JsonResult UserData()
        {
            Client currentUser = this.GetLoggedUser();
            var medicalRecord = this.medicalRecordRepository.GetMedicalRecords().Where(m => m.ClientID == currentUser.ClientID).FirstOrDefault();
            medicalRecord.Client = null;

            if (currentUser != null && medicalRecord != null)
            {
                return Json(new { Result = "OK", Data = medicalRecord }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Result = "NOOK", Data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
        }


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

    }
}