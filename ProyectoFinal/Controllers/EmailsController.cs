using API.Services;
using ProyectoFinal.Filters;
using ProyectoFinal.Models;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin", OtherRole = "Instructor")]
    [HandleError()]
    public class EmailsController : Controller
    {
        private String _templatePath;
        private String _finalTemplate;

        public String TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = value; }
        }

        public String FinalTempalte
        {
            get { return _finalTemplate; }
            set { _finalTemplate = value; }
        }

        #region Properties
        private IPaymentRepository paymentRepository;
        private IPaymentTypeRepository paymentTypeRepository;
        private IClientRepository clientRepository;
        private IActivityRepository activityRepository;
        #endregion

        #region Constructors
        public EmailsController()
        {
            this.paymentRepository = new PaymentRepository(new GymContext());
            this.paymentTypeRepository = new PaymentTypeRepository(new GymContext());
            this.clientRepository = new ClientRepository(new GymContext());
            this.activityRepository = new ActivityRepository(new GymContext());
        }

        public EmailsController(IPaymentRepository paymentRepository, IPaymentTypeRepository paymentTypeRepository, 
                                IClientRepository clientRepository, IActivityRepository activityRepository)
        {
            this.paymentRepository = paymentRepository;
            this.paymentTypeRepository = paymentTypeRepository;
            this.clientRepository = clientRepository;
            this.activityRepository = activityRepository;
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Preview(EmailViewModel model)
        {
            string result = "OK";
            string data = "some template";
            

            if (ModelState.IsValid)
            {
                this.FinalTempalte = this.BuildTemplate(model);
                data = this.FinalTempalte;
                Session["FinalTemplate"] = this.FinalTempalte;
            }
            else
            {
                result = "NOOK";
                data = string.Empty;
            }

            return Json(new { Result = result, Data = data }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Destinatarios()
        {
            IEnumerable<DestinatariesViewModel> options = this.BuildDestinatariesListOptions();
            return View(options);
        }

        public JsonResult SendEmailToClients(DestinataryTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IEnumerable<String> emails = this.FindDestinatariesByCategory(model);
                    Session["DestinatariesEmails"] = emails;
                    String template = Session["FinalTemplate"].ToString();

                    if (emails.Count() == 0)
                    {
                        return Json(new { Result = "NOOK_EMPTY", Redirect = string.Empty, Error = "El grupo seleccionado aún no posee destinatarios", Description = string.Empty });
                    }

                    if (!String.IsNullOrEmpty(template))
                    {
                        SendGridMailing sg = new SendGridMailing();
                        sg.Execute(template, emails.Distinct().ToList());
                    }
                    return Json(new { Result = "OK", Redirect = "/Emails/ThankYouPage", Error = string.Empty, Description = string.Empty });
                }
                return Json(new { Result = "NOOK", Redirect = string.Empty, Error = "Debe seleccionar al menos un grupo como destinatario", Description = string.Empty });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "NOOK", Redirect = string.Empty, Error = "Ha surgido un error inesperado. Por favor vuelva a intentar", Description = ex.Message });
            }
        }

        public ViewResult ThankYouPage()
        {
            return View();
        }

        #region PrivateMethods
        private string BuildTemplate(EmailViewModel model)
        {
            const string TITLE = "REPLACE_TEXT_TITLE";
            const string SUBTITLE = "REPLACE_TEXT_SUBTITLE";
            const string INNERTITLE = "REPLACE_TEXT_INNER";
            const string DESC = "REPLACE_TEXT_DESCRIPTION";
            this.TemplatePath = Server.MapPath(@"~/Templates/EmailTemplate.html");

            var template = System.IO.File.ReadAllText(TemplatePath);
            template = template.Replace(TITLE, model.Title)
                               .Replace(SUBTITLE, model.SubTitle)
                               .Replace(INNERTITLE, model.InnerTitle)
                               .Replace(DESC, model.Description);

            return template;
        }

        private IEnumerable<DestinatariesViewModel> BuildDestinatariesListOptions()
        {
            int idOption = 99;
            List<DestinatariesViewModel> options = new List<DestinatariesViewModel>();

            var activities = this.activityRepository.GetActivities();
            foreach (var activity in activities)
            {
                options.Add(new DestinatariesViewModel { ID = activity.ActivityID, Name = "Abonados de " + activity.Name, DestinataryType = Utils.Catalog.Destinataries.Activity });
            }

            foreach (Utils.Catalog.Destinataries destinataryType in Enum.GetValues(typeof(Utils.Catalog.Destinataries)))
            {
                if (destinataryType == Utils.Catalog.Destinataries.Activity)
                    continue;

                if (destinataryType != Utils.Catalog.Destinataries.ClientsWithDebt)
                {
                    options.Add(new DestinatariesViewModel { ID = ++idOption, Name = destinataryType.ToString(), DestinataryType = destinataryType });
                }
                else
                {
                    options.Add(new DestinatariesViewModel { ID = ++idOption, Name = "Clientes con deuda", DestinataryType = destinataryType });
                }
            }
            return options;
        }

        private IEnumerable<String> FindDestinatariesByCategory(DestinataryTypeViewModel model)
        {
            IEnumerable<String> emails = new List<string>();
            Utils.Catalog.Destinataries destinataryCategory = (Utils.Catalog.Destinataries)System.Enum.Parse(typeof(Utils.Catalog.Destinataries), model.Type);

            if (destinataryCategory == Utils.Catalog.Destinataries.Activity)
            {
                emails = this.GetDestinataries(destinataryCategory, model.ID);
            }
            else
            {
                emails = this.GetDestinataries(destinataryCategory);
            }

            return emails;
        }

        private IEnumerable<String> GetDestinataries(Utils.Catalog.Destinataries destinataryCategory, int id = 0)
        {
            IEnumerable<String> emails = new List<string>();
            if (destinataryCategory == Utils.Catalog.Destinataries.Activity)
            {
                emails = paymentRepository.GetClientsByActivity(id);
            }
            else if (destinataryCategory == Utils.Catalog.Destinataries.ClientsWithDebt)
            {
                emails = clientRepository.GetClientsWithDebt();
            }
            else if (destinataryCategory == Utils.Catalog.Destinataries.Admins)
            {
                emails = clientRepository.GetClients().Where(c => c.Role == Utils.Catalog.Roles.Admin).Select(c => c.Email).Distinct().ToList();
            }
            else if (destinataryCategory == Utils.Catalog.Destinataries.Profesores)
            {
                emails = clientRepository.GetClients().Where(c => c.Role == Utils.Catalog.Roles.Instructor).Select(c => c.Email).Distinct().ToList();
            }
            else if (destinataryCategory == Utils.Catalog.Destinataries.Clientes)
            {
                emails = clientRepository.GetClients().Where(c => c.Role == Utils.Catalog.Roles.Client).Select(c => c.Email).Distinct().ToList();
            }

            return emails;
        }
        #endregion

    }
}