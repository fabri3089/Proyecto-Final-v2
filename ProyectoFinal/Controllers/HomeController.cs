using API.Services;
using ProyectoFinal.Filters;
using ProyectoFinal.Models;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Models.ViewModels;
using ProyectoFinal.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoFinal.Controllers
{
    [HandleError()]
    public class HomeController : Controller
    {
        private IClientRepository clientRepository;
        private IAssistanceRepository assistanceRepository;

        public HomeController()
        {
            this.clientRepository = new ClientRepository(new GymContext());
            this.assistanceRepository = new AssistanceRepository(new GymContext());
        }

        public HomeController(IClientRepository clientRepository, IAssistanceRepository assistanceRepository)
        {
            this.clientRepository = clientRepository;
            this.assistanceRepository = assistanceRepository;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Client client = Authenticate(model.Email, model.Password);
                if (client != null)
                {
                    Session["User"] = client;
                    Session["UserName"] = client.Email;
                    Session["Role"] = client.Role;

                    //TODO Pagina personal del cliente logueado
                    var returnUrl = Session["ReturnURL"];
                    if (returnUrl != null && returnUrl.ToString() != string.Empty)
                    {
                        return Redirect(Url.Action("Index", returnUrl.ToString()));
                    }
                    else
                    {
                        return Redirect(Url.Action("Index", "Clients"));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return View("Index");
        }

        [HttpGet]
        public ActionResult Access()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Access(string docNumber)
        {
            #region Validations
            int documentNumber;
            bool parseResult = Int32.TryParse(docNumber, out documentNumber);

            if (!parseResult || docNumber.Length != 8)
            {
                ModelState.AddModelError("Format", "Debes ingresar un número de documento");
                return View();
            }
            #endregion

            Client client = clientRepository.GetClients().Where(c => c.DocNumber == documentNumber && c.Role==Catalog.Roles.Client).FirstOrDefault();

            //var result = clientRepository.ListOfPayments(client);

            if (client == null || (client.Role != Catalog.Roles.Client))
            {
                //No se encontró un cliente con los datos ingresados o bien no es cliente, es admin o profesor
                ModelState.AddModelError("Format", "No se encontró ningún socio con el nro de documento ingresado");
                return View();
            }
            else if(clientRepository.HasActivePayment(client))
            {
                //OK
                Assistance assistance = new Assistance { assistanceDate = DateTime.Now, ClientID = client.ClientID };
                assistanceRepository.InsertAssistance(assistance);
                assistanceRepository.Save();
                ViewBag.IsEnabled = true;
            }
            else
            {
                ViewBag.IsEnabled = false;
                //No tiene abono activo
            }
            return View();
        }

        private Client Authenticate(string username, string password)
        {
            Client client = clientRepository.GetClients().Where(c => c.Email == username).FirstOrDefault();
            if (client == null)
            {}
            else if (PasswordUtilities.Compare(password, client.Password, client.PasswordSalt))
                return client;

            return null;
        }

        //[HttpGet]
        //[AuthorizationPrivilege(Role = "Admin")]
        //public ActionResult EmailAndSMS()
        //{
        //    SMS smsService = new SMS();

        //    var accountSid = Environment.GetEnvironmentVariable("ENVIRONMENT_VARIABLE_ACCOUNT_SID");
        //    var authToken = Environment.GetEnvironmentVariable("ENVIRONMENT_VARIABLE_AUTH_TOKEN");

        //    smsService.Execute(accountSid, authToken, "+1 256-305-4229", "+5492355677581", "Testing via C# !");

        //    SendGridMailing sg = new SendGridMailing();

        //    //var templatePath = Server.MapPath(@"~/Templates/MailTemplate.html");
        //    var templatePath = Server.MapPath(@"~/Templates/EmailBienvenida.html");
        //    sg.Execute(templatePath, "cristian.pique@hotmail.com", "335588");

        //    return View("Index");
        //}


    }


}