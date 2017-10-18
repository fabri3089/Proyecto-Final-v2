using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ProyectoFinal.Models;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Controllers;
using Telerik.JustMock;
using System.Linq;
using System.Web.Mvc;

namespace ProyectoFinal.Tests
{
    [TestClass]
    public class ActivitySchedulesControllerTest
    {
        List<Group> activitySchedules;
        List<Activity> activities;
        Group newActivitySchedule;
        IGroupRepository activitySchedulesRepository;
        IActivityRepository activityRepository;
        GroupsController controller;
        public const int ACTIVITY_SCHEDULE_ID_TO_USE = 1;
        public const int ACTIVITY_SCHEDULE_ID_NOT_FOUND = 33000;

        [TestInitialize]
        public void Init()
        {
            #region Dummy Activity && ActivitySchedules Lists
            activities = new List<Activity>
            {
                new Activity { ActivityID = 1, Name = "Gimnasio", Description = "Gimnasio, pesas, bicicletas, máquinas para correr"},
                new Activity { ActivityID = 2, Name = "Pilates", Description = "Sistema de entrenamiento físico y mental"},
                new Activity { ActivityID = 3, Name = "Boxeo", Description = "Deporte de combate"}
            };


           /* activitySchedules = new List<Group>
            {
                new Group { ActivityScheduleID = 1, ActivityID = 1, Day = "Lunes a Viernes", HourFrom = 08, HourTo = 22, Activity = activities.ElementAt(0)},
                new Group { ActivityScheduleID = 2, ActivityID = 2, Day = "Lunes", HourFrom = 11, HourTo = 12, Activity = activities.ElementAt(1)},
                new Group { ActivityScheduleID = 3, ActivityID = 2, Day = "Lunes", HourFrom = 16, HourTo = 17, Activity = activities.ElementAt(1)},
                new Group { ActivityScheduleID = 4, ActivityID = 2, Day = "Miércoles", HourFrom = 19, HourTo = 21, Activity = activities.ElementAt(2)},
                new Group { ActivityScheduleID = 5, ActivityID = 3, Day = "Miércoles", HourFrom = 9, HourTo = 10, Activity = activities.ElementAt(2)},
                new Group { ActivityScheduleID = 6, ActivityID = 3, Day = "Miércoles", HourFrom = 20, HourTo = 21, Activity = activities.ElementAt(1)},
            };
            */
            #endregion

            #region Dummy New Activity
            newActivitySchedule = new Group { ActivityID = 1, Day = "Lunes y Jueves", HourFrom = 16, HourTo = 18 };
            #endregion

            #region Repositories
            activitySchedulesRepository = Mock.Create<IGroupRepository>();
            activityRepository = Mock.Create<IActivityRepository>();
            #endregion

            #region JustMock GetActivities Arrange
            Mock.Arrange(() => activitySchedulesRepository.GetActivitySchedules()).Returns(activitySchedules);
            Mock.Arrange(() => activitySchedulesRepository.GetActivityScheduleByID(ACTIVITY_SCHEDULE_ID_TO_USE))
                                                 .Returns(activitySchedules.Where(a => a.ActivityID == ACTIVITY_SCHEDULE_ID_TO_USE).FirstOrDefault());
            Mock.Arrange(() => activitySchedulesRepository.InsertActivitySchedule(newActivitySchedule))
                                                 .DoInstead(() => activitySchedules.Add(newActivitySchedule))
                                                 .MustBeCalled();
            Mock.Arrange(() => activitySchedulesRepository.DeleteActivitySchedule(ACTIVITY_SCHEDULE_ID_TO_USE))
                                     .DoInstead(() => activitySchedules.Remove(activitySchedules.Where(a => a.ActivityScheduleID == ACTIVITY_SCHEDULE_ID_TO_USE).FirstOrDefault()));
            Mock.Arrange(() => activitySchedulesRepository.Save()).DoNothing();
            #endregion

            #region Controller creation
            controller = new GroupsController(activitySchedulesRepository, activityRepository);
            #endregion
        }

        [TestMethod]
        public void ActivityShedules_Index()
        {
            // Arrange
            ViewResult viewResult = controller.Index(string.Empty, string.Empty, string.Empty, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Group>;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Count(), activitySchedules.Count);
        }

        [TestMethod]
        public void ActivityShedules_IndexWithSort()
        {
            const string SORT_ORDER_DESC = "name_desc"; //change for name_asc if necessary
            const string SORT_MODE_DESC = "desc"; //change for asc if necessary

            ViewResult viewResult = controller.Index(SORT_ORDER_DESC, string.Empty, string.Empty, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Group>;

            if (SORT_MODE_DESC == "desc")
                activitySchedules = activitySchedules.OrderByDescending(a => a.Activity.Name).ToList();

            //Assert
            Assert.IsNotNull(model);
            Assert.IsTrue(activitySchedules.FirstOrDefault().Equals(model.FirstOrDefault()));
            Assert.AreEqual(activitySchedules.Take(model.Count()).Last().Activity.Name, model.Last().Activity.Name);
        }

        [TestMethod]
        public void ActivityShedules_IndexWithSearch()
        {
            const string SEARCHSTRING = "Boxeo";

            ViewResult viewResult = controller.Index(string.Empty, string.Empty, SEARCHSTRING, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Group>;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Except(model.Where(a => a.Activity.Name.Contains(SEARCHSTRING))).Count(), 0);
        }

        [TestMethod]
        public void ActivityShedules_Details()
        {
            var activity = activitySchedules.Where(a => a.ActivityID == ACTIVITY_SCHEDULE_ID_TO_USE).FirstOrDefault();

            ViewResult viewResult = controller.Details(ACTIVITY_SCHEDULE_ID_TO_USE) as ViewResult;
            var model = viewResult.Model as Group;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.ActivityID, ACTIVITY_SCHEDULE_ID_TO_USE);
            Assert.AreEqual(model.ActivityID, activity.ActivityID);
            Assert.AreEqual(model.HourFrom, activity.HourFrom);
            Assert.AreEqual(model.HourTo, activity.HourTo);
        }

        [TestMethod]
        public void ActivityShedules_Create()
        {
            int countActivitiesBefore = activitySchedules.Count;

            ActionResult actionResult = controller.Create(newActivitySchedule);

            Assert.AreNotEqual(countActivitiesBefore, activitySchedules.Count);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void ActivityShedules_Edit_HttpGet()
        {
            ActionResult actionResult = controller.Edit(ACTIVITY_SCHEDULE_ID_TO_USE);
            var model = (actionResult as ViewResult).Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));

        }

        [TestMethod]
        public void ActivityShedules_Edit_HttpGet_HttpNotFound()
        {
            Mock.Arrange(() => activitySchedulesRepository.GetActivityScheduleByID(ACTIVITY_SCHEDULE_ID_NOT_FOUND))
                                                          .Returns(activitySchedules.Where(a => a.ActivityScheduleID == ACTIVITY_SCHEDULE_ID_NOT_FOUND).FirstOrDefault());

            ActionResult actionResult = controller.Edit(33000); //A very high ActivityID

            var result = actionResult as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
            Assert.IsNotNull(result.GetType().GetProperty("StatusDescription"), null);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void ActivityShedules_Edit_HttpPost()
        {
            var originalActivity = activitySchedules.FirstOrDefault();
            var originalName = activitySchedules.FirstOrDefault().Activity.Name;
            var originalHourFrom = activitySchedules.FirstOrDefault().HourFrom;
            Mock.Arrange(() => activitySchedulesRepository.Save())
                                                          .DoInstead(() => { originalActivity.Activity.Name = "Changed!"; originalActivity.HourFrom = 11; });

            ActionResult actionResult = controller.Edit(newActivitySchedule) as ActionResult;

            Assert.AreNotEqual(originalActivity.Activity.Name, originalName);
            Assert.AreNotEqual(originalActivity.HourFrom, originalHourFrom);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void ActivityShedules_Delete()
        {
            int totalActivitySchedulesBefore = activitySchedules.Count;
            var idToDelete = activitySchedules.FirstOrDefault().ActivityScheduleID;

            controller.DeleteConfirmed(activitySchedules.FirstOrDefault().ActivityScheduleID);

            //Assert
            Assert.IsTrue(totalActivitySchedulesBefore > activitySchedules.Count);
            Assert.IsFalse(activitySchedules.Any(a => a.ActivityScheduleID == idToDelete));
        }
    }
}
