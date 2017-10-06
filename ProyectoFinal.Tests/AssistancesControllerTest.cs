using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProyectoFinal.Models;
using System.Collections.Generic;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Controllers;
using Telerik.JustMock;
using System.Web.Mvc;
using System.Linq;

namespace ProyectoFinal.Tests
{
    [TestClass]
    public class AssistancesControllerTest
    {
        List<Assistance> assistances;
        Assistance newAssistance;
        IAssistanceRepository assistanceRepository;
        AssistancesController controller;
        public const int ASSISTANCE_ID_TO_USE = 1;
        public const int ASSISTANCE_ID_NOT_FOUND = 33000;

        [TestInitialize]
        public void Init()
        {
            #region Dummy Assistance List
            assistances = new List<Assistance>
            {
                new Assistance { AssistanceID = 1, assistanceDate = new DateTime(2016,01,01), ClientID = 1 },
                new Assistance { AssistanceID = 2, assistanceDate = new DateTime(2015,01,01), ClientID = 1 },
                new Assistance { AssistanceID = 3, assistanceDate = new DateTime(2016,02,01), ClientID = 1 },
                new Assistance { AssistanceID = 4, assistanceDate = new DateTime(2016,01,01), ClientID = 2 },
                new Assistance { AssistanceID = 5, assistanceDate = new DateTime(2015,01,01), ClientID = 2 },
                new Assistance { AssistanceID = 6, assistanceDate = new DateTime(2016,02,01), ClientID = 2 },
                new Assistance { AssistanceID = 7, assistanceDate = new DateTime(2016,01,01), ClientID = 3 },
                new Assistance { AssistanceID = 8, assistanceDate = new DateTime(2015,01,01), ClientID = 3 },
                new Assistance { AssistanceID = 9, assistanceDate = new DateTime(2016,02,01), ClientID = 3 },
                new Assistance { AssistanceID = 10, assistanceDate = new DateTime(2015,01,01), ClientID = 4 },
                new Assistance { AssistanceID = 11, assistanceDate = new DateTime(2016,02,01), ClientID = 4 },
                new Assistance { AssistanceID = 12, assistanceDate = new DateTime(2015,01,01), ClientID = 5 },
                new Assistance { AssistanceID = 13, assistanceDate = new DateTime(2016,02,01), ClientID = 5 },
                new Assistance { AssistanceID = 14, assistanceDate = new DateTime(2016,02,01), ClientID = 6 },
                new Assistance { AssistanceID = 15, assistanceDate = new DateTime(2016,02,01), ClientID = 6 },
                new Assistance { AssistanceID = 16, assistanceDate = new DateTime(2016,02,01), ClientID = 6 },
                new Assistance { AssistanceID = 17, assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { AssistanceID = 18, assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { AssistanceID = 19, assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { AssistanceID = 20, assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { AssistanceID = 21, assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { AssistanceID = 22, assistanceDate = new DateTime(2016,02,01), ClientID = 7 },
                new Assistance { AssistanceID = 23, assistanceDate = new DateTime(2016,02,01), ClientID = 7 },
                new Assistance { AssistanceID = 24, assistanceDate = new DateTime(2016,02,01), ClientID = 7 }
            };
            #endregion

            #region Dummy New Assistance
            newAssistance = new Assistance { AssistanceID = 25, assistanceDate = new DateTime(2016, 02, 01), ClientID = 7 };
            #endregion


            #region Repositories
            assistanceRepository = Mock.Create<IAssistanceRepository>();
            #endregion

            #region JustMock Assistances Arrange
            Mock.Arrange(() => assistanceRepository.GetAssistances()).Returns(assistances);
            Mock.Arrange(() => assistanceRepository.GetAssistanceByID(ASSISTANCE_ID_TO_USE))
                                                 .Returns(assistances.Where(a => a.AssistanceID == ASSISTANCE_ID_TO_USE).FirstOrDefault());
            Mock.Arrange(() => assistanceRepository.InsertAssistance(newAssistance))
                                                 .DoInstead(() => assistances.Add(newAssistance))
                                                 .MustBeCalled();
            Mock.Arrange(() => assistanceRepository.DeleteAssistance(ASSISTANCE_ID_TO_USE))
                                                   .DoInstead(() => assistances.Remove(assistances.Where(a => a.AssistanceID == ASSISTANCE_ID_TO_USE).FirstOrDefault()));
            Mock.Arrange(() => assistanceRepository.Save()).DoNothing();
            #endregion

            #region Controller creation
            controller = new AssistancesController(assistanceRepository);
            #endregion
        }

        [TestMethod]
        public void Assistance_Index()
        {
            // Arrange
            ViewResult viewResult = controller.Index(1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Assistance>;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.Count(), assistances.Count);
        }

        [TestMethod]
        public void Assistance_Details()
        {
            var assistance = assistances.Where(a => a.AssistanceID == ASSISTANCE_ID_TO_USE).FirstOrDefault();

            ViewResult viewResult = controller.Details(ASSISTANCE_ID_TO_USE) as ViewResult;
            var model = viewResult.Model as Assistance;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.AssistanceID, ASSISTANCE_ID_TO_USE);
            Assert.AreEqual(model.AssistanceID, assistance.AssistanceID);
            Assert.AreEqual(model.assistanceDate, assistance.assistanceDate);
        }

        [TestMethod]
        public void Assistance_Create()
        {
            int countassistancesBefore = assistances.Count;

            ActionResult actionResult = controller.Create(newAssistance);

            Assert.AreNotEqual(countassistancesBefore, assistances.Count);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Assistance_Edit_HttpGet()
        {
            ActionResult actionResult = controller.Edit(ASSISTANCE_ID_TO_USE);
            var model = (actionResult as ViewResult).Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));

        }

        [TestMethod]
        public void Assistance_Edit_HttpGet_HttpNotFound()
        {
            Mock.Arrange(() => assistanceRepository.GetAssistanceByID(ASSISTANCE_ID_NOT_FOUND))
                                                   .Returns(assistances.Where(a => a.AssistanceID == ASSISTANCE_ID_NOT_FOUND).FirstOrDefault());

            ActionResult actionResult = controller.Edit(33000); //A very high AssistanceID

            var result = actionResult as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
            Assert.IsNotNull(result.GetType().GetProperty("StatusDescription"), null);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void Assistance_Edit_HttpPost()
        {
            var originalAssistance = assistances.FirstOrDefault();
            var originalDate = assistances.FirstOrDefault().assistanceDate;
            Mock.Arrange(() => assistanceRepository.Save()).DoInstead(() => { originalAssistance.assistanceDate = DateTime.Now; });

            ActionResult actionResult = controller.Edit(newAssistance) as ActionResult;

            Assert.AreNotEqual(originalAssistance.assistanceDate, originalDate);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Assistance_Delete()
        {
            int totalassistancesBefore = assistances.Count;
            var idToDelete = assistances.FirstOrDefault().AssistanceID;

            controller.DeleteConfirmed(assistances.FirstOrDefault().AssistanceID);

            //Assert
            Assert.IsTrue(totalassistancesBefore > assistances.Count);
            Assert.IsFalse(assistances.Any(a => a.AssistanceID == idToDelete));
        }
    }
}
