using ProyectoFinal.Filters;
using ProyectoFinal.Models;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinal.Controllers
{
    [AuthorizationPrivilege(Role = "Admin")]
    [HandleError()]
    public class StatisticsController : Controller
    {
        #region Properties
        private IAssistanceRepository assistanceRepository;
        private IClientRepository clientRepository;
        private IPaymentRepository paymentRepository;
        private IActivityRepository activityRepository;
        #endregion

        #region Constructors
        public StatisticsController()
        {
            this.assistanceRepository = new AssistanceRepository(new GymContext());
            this.clientRepository = new ClientRepository(new GymContext());
            this.paymentRepository = new PaymentRepository(new GymContext());
            this.activityRepository = new ActivityRepository(new GymContext());
        }

        public StatisticsController(IAssistanceRepository assistanceRepository, IClientRepository clientRepository, 
                                    IPaymentRepository paymentRepository, IActivityRepository activityRepository)
        {
            this.assistanceRepository = assistanceRepository;
            this.clientRepository = clientRepository;
            this.paymentRepository = paymentRepository;
            this.activityRepository = activityRepository;
        }
        #endregion

        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra asistencias del último año clasificando las edades
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult BarChart()
        {
            Men men = new Men(); 
            Women women = new Women();
            List<int> totalHombres = new List<int>();
            List<int> totalMujeres = new List<int>();
            
            var assistances = assistanceRepository.GetAssistances().Where(a => a.assistanceDate.Year == DateTime.Now.Year).ToList();

            try
            {
                foreach (var item in assistances)
                {
                    #region GetCliente
                    Client c = clientRepository.GetClientByID(item.ClientID);
                    if (c == null)
                        continue;
                    #endregion

                    #region Calculo de Edad,  de Clasificacion en Edad y lo cuento
                    var today = DateTime.Today;
                    var age = today.Year - c.BirthDate.Year;
                    if (c.BirthDate > today.AddYears(-age)) age--;

                    if (c.Sexo.Equals(Utils.Catalog.Genre.Hombre))
                    {
                        if (age < 36) { men.YoungAdult++; }
                        else if (age < 51) { men.Adult++; }
                        else if (age < 65) { men.MiddleAge++; }
                        else { men.Senior++; }
                    }
                    else
                    {
                        if (age < 36) { women.YoungAdult++; }
                        else if (age < 51) { women.Adult++; }
                        else if (age < 65) { women.MiddleAge++; }
                        else { women.Senior++; }
                    }
                    #endregion
                }

                #region Listas para el front
                totalHombres.Insert(0, men.YoungAdult);
                totalHombres.Insert(1, men.Adult);
                totalHombres.Insert(2, men.MiddleAge);
                totalHombres.Insert(3, men.Senior);
                totalMujeres.Insert(0, women.YoungAdult);
                totalMujeres.Insert(1, women.Adult);
                totalMujeres.Insert(2, women.MiddleAge);
                totalMujeres.Insert(3, women.Senior);
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { Result = "NOOK", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = "OK", Men = totalHombres, Women = totalMujeres }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Muestra cantidad de inscripciones al gimnasio por año
        /// Toma en cuenta la primera vez que un socio acude al gimnasio y se registra
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult LineChart() {
            var clients = clientRepository.GetClients().ToList();
            List<LineChartItem> response = new List<LineChartItem>();

            try
            {
                #region Load years
                var minYear = Convert.ToInt32(ConfigurationManager.AppSettings["OpenYear"]);
                var maxYear = DateTime.Now.Year;

                for (int i = minYear; i <= maxYear; i++)
                {
                    response.Add(new LineChartItem { Year = i, CantAbonos = 0 });
                }
                #endregion

                #region Load CantOfPayments 
                foreach (var client in clients)
                {
                    foreach (var lineChartItem in response)
                    {
                        if (client.DateFrom.Year == lineChartItem.Year)
                        {
                            lineChartItem.CantAbonos++;
                            continue;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { Result = "NOOK", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = "OK", Data = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Muestra cantidad de abonos históricos registrados por actividad
        /// Muestra el impacto de una actividad sobre los ingresos totales del gimnasio
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult PieChart()
        {
            List<PieChartItem> response = new List<PieChartItem>();

            try
            {
                var activities = activityRepository.GetActivities().ToList();
                var payments = paymentRepository.GetPayments().OrderBy(p => p.PaymentID).ToList();

                foreach (var activity in activities)
                {
                    response.Add(new PieChartItem { ActivityID = activity.ActivityID, ActivityName = activity.Name, CantAbonos = 0 });
                }
                foreach (var payment in payments)
                {
                    response.Where(r => r.ActivityID == payment.PaymentType.ActivityID).FirstOrDefault().CantAbonos++;
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "NOOK", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = "OK", Data = response }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Muestra asistencia HISTORICA clasificadas en meses, para conocer meses con más tráfico y patrones estacionales
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult HooksChart()
        {
            #region Initialize response
            List<HooksChartItem> response = new List<HooksChartItem>();
            for (int month = 1; month <= 12; month++)
            {
                response.Add(new HooksChartItem { Attendance = 0, Month = month });
            }
            #endregion

            try
            {
                var assistances = assistanceRepository.GetAssistances().ToList();
                foreach (var assist in assistances)
                {
                    response.Where(h => h.Month == assist.assistanceDate.Month).FirstOrDefault().Attendance++;
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "NOOK", Error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Result = "OK", Data = response }, JsonRequestBehavior.AllowGet);
        }
    

        #region Classes for Json response
        #region BarChart classes
        private class Men
        {
            public int YoungAdult { get; set; }
            public int Adult { get; set; }
            public int MiddleAge { get; set; }
            public int Senior { get; set; }
        }

        private class Women
        {
            public int YoungAdult { get; set; }
            public int Adult { get; set; }
            public int MiddleAge { get; set; }
            public int Senior { get; set; }
        }
        #endregion

        #region LineChart classes
        public class LineChartItem
        {
            public int Year { get; set; }
            public int CantAbonos { get; set; }
        }
        #endregion

        #region PieChart classes
        public class PieChartItem
        {
            public int ActivityID { get; set; }
            public string ActivityName { get; set; }
            public int CantAbonos { get; set; }
        }
        #endregion

        #region 
        public class HooksChartItem
        {
            public int Month { get; set; }
            public int Attendance { get; set; }
        }
        #endregion
        #endregion
    }
}