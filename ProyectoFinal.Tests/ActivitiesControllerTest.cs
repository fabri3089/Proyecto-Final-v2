using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProyectoFinal.Models;
using System.Collections.Generic;
using ProyectoFinal.Controllers;
using System.Web.Mvc;
using ProyectoFinal.Models.Repositories;
using Telerik.JustMock;
using ProyectoFinal.Utils;
using System.Linq;

namespace ProyectoFinal.Tests
{
    [TestClass]
    public class ActivitiesControllerTest
    {
        List<Activity> activities;
        Activity newActivity;
        IActivityRepository activityRepository;
        IPaymentTypeRepository paymentTypeRepository;
        ActivitiesController controller;
        public const int ACTIVITY_ID_TO_USE = 1;
        public const int ACTIVITY_ID_NOT_FOUND = 33000;

        [TestInitialize]
        public void Init()
        {
            #region Dummy Activity List
            activities = new List<Activity>
            {
                new Activity { ActivityID = 1, Name = "Gimnasio", Description = "Gimnasio, pesas, bicicletas, máquinas para correr"},
                new Activity { ActivityID = 2, Name = "Pilates", Description = "Sistema de entrenamiento físico y mental"},
                new Activity { ActivityID = 3, Name = "Boxeo", Description = "Deporte de combate"},
                new Activity { ActivityID = 4, Name = "Body Pump",
                               Description = "Actividad dirigida que combina ejercicios propios de la sala de musculación (con mancuernas, barras y discos) con la                                      mejor música que permite lograr una completa actividad aeróbica"},
                new Activity { Name = "Fit-ball", Description = "Es ﻿un sistema de entrenamiento físico diseñado sobre los principios de la fisioterapia. Consiste en realizar ejercicios con una pelota inflable o fit-ball."}
            };


            #endregion

            #region Dummy New Activity
            newActivity = new Activity { Name = "X55", Description = "Es un moderno y revolucionario programa de resistencia localizada. Son 55 minutos de extrema energía e intensidad,                                                           guiados por una música específica. " };
            #endregion

            #region Dummy PaymentType List
            var paymentType = new List<PaymentType>
            {
                new PaymentType { Description = "Gimnasio mensual", Status = Catalog.Status.Active, ActivityID = 1, DurationInMonths = 1 },
                new PaymentType { Description = "Gimnasio anual", Status = Catalog.Status.Active, ActivityID = 1, DurationInMonths = 12 },
                new PaymentType { Description = "Pilates mensual", Status = Catalog.Status.Active, ActivityID = 2, DurationInMonths = 1 },
                new PaymentType { Description = "Boxeo mensual", Status = Catalog.Status.Active, ActivityID = 3, DurationInMonths = 1 },
            };
            #endregion

            #region Repositories
            activityRepository = Mock.Create<IActivityRepository>();
            paymentTypeRepository = Mock.Create<IPaymentTypeRepository>();
            #endregion

            #region JustMock GetActivities Arrange
            Mock.Arrange(() => activityRepository.GetActivities()).Returns(activities);
            Mock.Arrange(() => activityRepository.GetActivityByID(ACTIVITY_ID_TO_USE))
                                                 .Returns(activities.Where(a => a.ActivityID == ACTIVITY_ID_TO_USE).FirstOrDefault());
            Mock.Arrange(() => activityRepository.InsertActivity(newActivity))
                                                 .DoInstead(() => activities.Add(newActivity))
                                                 .MustBeCalled();
            Mock.Arrange(() => activityRepository.DeleteActivity(ACTIVITY_ID_TO_USE))
                                                 .DoInstead(() => activities.Remove(activities.Where(a => a.ActivityID == ACTIVITY_ID_TO_USE).FirstOrDefault()));
            Mock.Arrange(() => activityRepository.Save()).DoNothing();
            #endregion

            #region Controller creation
            controller = new ActivitiesController(activityRepository, paymentTypeRepository);
            #endregion
        }

        [TestMethod]
        public void Activity_Index()
        {
            // Arrange
            ViewResult viewResult = controller.Index(string.Empty, string.Empty, string.Empty, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Activity>;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Count(), activities.Count);
        }

        [TestMethod]
        public void Activity_IndexWithSort()
        {
            const string SORT_ORDER_DESC = "name_desc"; //change for name_asc if necessary
            const string SORT_MODE_DESC = "desc"; //change for asc if necessary

            ViewResult viewResult = controller.Index(SORT_ORDER_DESC, string.Empty, string.Empty, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Activity>;

            if (SORT_MODE_DESC == "desc")
                activities = activities.OrderByDescending(c => c.Name).ToList();

            //Assert
            Assert.IsNotNull(model);
            Assert.IsTrue(activities.FirstOrDefault().Equals(model.FirstOrDefault()));
            Assert.AreEqual(activities.Take(model.Count()).Last().Name, model.Last().Name);
        }

        [TestMethod]
        public void Activity_IndexWithSearch()
        {
            const string SEARCHSTRING = "Boxeo";

            ViewResult viewResult = controller.Index(string.Empty, string.Empty, SEARCHSTRING, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Activity>;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Except(model.Where(a => a.Name.Contains(SEARCHSTRING))).Count(), 0);
        }

        [TestMethod]
        public void Activity_Details()
        {
            var activity = activities.Where(a => a.ActivityID == ACTIVITY_ID_TO_USE).FirstOrDefault();

            ViewResult viewResult = controller.Details(ACTIVITY_ID_TO_USE) as ViewResult;
            var model = viewResult.Model as Activity;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.ActivityID, ACTIVITY_ID_TO_USE);
            Assert.AreEqual(model.ActivityID, activity.ActivityID);
            Assert.AreEqual(model.Description, activity.Description);
        }

        [TestMethod]
        public void Activity_Create()
        {
            int countActivitiesBefore = activities.Count;

            ActionResult actionResult = controller.Create(newActivity);

            Assert.AreNotEqual(countActivitiesBefore, activities.Count);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Activity_Edit_HttpGet()
        {
            ActionResult actionResult = controller.Edit(ACTIVITY_ID_TO_USE);
            var model = (actionResult as ViewResult).Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));

        }

        [TestMethod]
        public void Activity_Edit_HttpGet_HttpNotFound()
        {
            Mock.Arrange(() => activityRepository.GetActivityByID(ACTIVITY_ID_NOT_FOUND)).Returns(activities.Where(a => a.ActivityID == ACTIVITY_ID_NOT_FOUND).FirstOrDefault());

            ActionResult actionResult = controller.Edit(33000); //A very high ActivityID

            var result = actionResult as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
            Assert.IsNotNull(result.GetType().GetProperty("StatusDescription"), null);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void Activity_Edit_HttpPost()
        {
            var originalActivity = activities.FirstOrDefault();
            var originalName = activities.FirstOrDefault().Name;
            var originalDesc = activities.FirstOrDefault().Description;
            Mock.Arrange(() => activityRepository.Save()).DoInstead(() => { originalActivity.Name = "Changed!"; originalActivity.Description = "Desc changed!"; });

            ActionResult actionResult = controller.Edit(newActivity) as ActionResult;

            Assert.AreNotEqual(originalActivity.Name, originalName);
            Assert.AreNotEqual(originalActivity.Description, originalDesc);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Activity_Delete()
        {
            int totalActivitiesBefore = activities.Count;
            var idToDelete = activities.FirstOrDefault().ActivityID;

            controller.DeleteConfirmed(activities.FirstOrDefault().ActivityID);

            //Assert
            Assert.IsTrue(totalActivitiesBefore > activities.Count);
            Assert.IsFalse(activities.Any(a => a.ActivityID == idToDelete));
        }
    }
}
