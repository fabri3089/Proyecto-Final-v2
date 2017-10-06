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
using System.Collections.Specialized;
using System.IO;
using ProyectoFinal.Filters;
using PagedList;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace ProyectoFinal.Controllers
{
    [HandleError()]
    public class FilesController : Controller
    {
        #region Properties
        private IFileRepository fileRepository;
        private IRoutineRepository routineRepository;
        #endregion

        #region Constructors
        public FilesController()
        {
            this.fileRepository = new FileRepository(new GymContext());
            this.routineRepository = new RoutineRepository(new GymContext());
        }

        public FilesController(IFileRepository fileRepository, IRoutineRepository routineRepository)
        {
            this.fileRepository = fileRepository;
            this.routineRepository = routineRepository;
        }
        #endregion

        // GET: Files
        public ActionResult Index(int id)
        {
            Session["RoutineID"] = id;
            Routine r = routineRepository.GetRoutineByID(id);
            ViewBag.RoutineName = r.NameFile;
            ViewBag.Description = r.Description;
            if (r.Client != null && string.IsNullOrEmpty(r.Client.FirstName) && string.IsNullOrEmpty(r.Client.LastName))
            {
                ViewBag.ClientName = r.Client.FirstName + ' ' + r.Client.LastName;
            }
            else
            {
                ViewBag.ClientName = string.Empty;
            }

            IEnumerable<Models.File> files;
            files = fileRepository.GetFiles().Where(f => f.RoutineID == id);

            return View(files);
        }

        public JsonResult Save(List<ProyectoFinal.Models.File> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    fileRepository.DeleteFilesByRoutineID((int)Session["RoutineID"]);
                    fileRepository.InsertListOfFiles(files);
                    fileRepository.Save();
                }
                else
                {
                    return Json(new { Result = "NOOK", Data = "Modelo inválido" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "NOOK", Data = ex.Message }, JsonRequestBehavior.AllowGet); 
            }

            return Json(new { Result = "OK", Data = "Save exitoso" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndexPDF(int id)
        {
            Routine r = routineRepository.GetRoutineByID(id);
            ViewBag.RoutineName = r.NameFile;
            ViewBag.Description = r.Description;
            if (r.Client != null && !string.IsNullOrEmpty(r.Client.FirstName) && !string.IsNullOrEmpty(r.Client.LastName))
            {
                ViewBag.ClientName = r.Client.FirstName + ' ' + r.Client.LastName;
            }
            else
            {
                ViewBag.ClientName = string.Empty;
            }

            IEnumerable<Models.File> files;
            files = fileRepository.GetFiles().Where(f => f.RoutineID == id);

            return View(files);
        }

        public FileResult GeneratePDF(int id)
        {
            string apiKey = ConfigurationManager.AppSettings["API_KEY"];
            string value = string.Format("http://amosgym.azurewebsites.net/Files/IndexPDF/{0}", id.ToString());

            using (var client = new WebClient())
            {
                NameValueCollection options = new NameValueCollection();
                options.Add("apikey", apiKey);
                options.Add("value", value);

                // Call the API convert to a PDF
                byte[] result = client.UploadValues("http://api.html2pdfrocket.com/pdf", options);

                // Download to the client
                return File(result, System.Net.Mime.MediaTypeNames.Application.Pdf, "MiRutina.pdf");

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fileRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        #region New
        [HttpGet]
        public JsonResult GetExercises(int? routineID)
        {
            var files = fileRepository.GetFiles();

            routineID = 1;
            var query =
                     fileRepository.GetFiles()
                      .Where(p => p.RoutineID == routineID)
                      .Select(p => new
                      {
                          NameFile = p.Routine.NameFile,
                          ExerciseName = p.ExerciseName,
                          MuscleName = p.MuscleName,
                          Peso = p.Peso,
                          Repetitions = p.Repetitions,
                          Day = p.Day
                      })
                      .ToList();

            return Json(new { Result = "OK", Data = query } , JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
