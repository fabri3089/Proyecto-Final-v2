using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Models;
using System.Collections.Generic;
using ProyectoFinal.Utils;
using ProyectoFinal.Controllers;
using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using System.Text.RegularExpressions;

namespace ProyectoFinal.Tests
{
    [TestClass]
    public class ClientsControllerTest
    {
        List<Client> clients;
        List<Client> massiveClients;
        Client newClient;
        IClientRepository clientRepository;
        ClientsController controller;
        public const int CLIENT_ID_TO_USE = 1;
        public const int CLIENT_ID_NOT_FOUND = 33154;

        [TestInitialize]
        public void Init()
        {
            #region Dummy Clients List
            var passwordSalt1 = PasswordUtilities.CreateSalt(16);
            var password1 = PasswordUtilities.GenerateSHA256Hash("12345", passwordSalt1);
            var passwordSalt2 = PasswordUtilities.CreateSalt(16);
            var password2 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt2);

            clients = new List<Client>
            {
                new Client { ClientID=1, FirstName = "John", LastName = "Doe", DocType = "DNI", DocNumber = 34578800, BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "john.doe@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1 },

                new Client { ClientID=2, FirstName = "Cristian", LastName = "Piqué", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1989, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "cristian.pique@hotmail.com",
                Password = password2, PasswordSalt = passwordSalt2 },

                new Client { ClientID=3, FirstName = "Ted", LastName = "Mosby", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1985, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "ted.mosby@gmail.com",
                Password = password2, PasswordSalt = passwordSalt2 }
            };
            #endregion

            #region Dummy IsolatedClient
            var passwordSalt = PasswordUtilities.CreateSalt(16);
            newClient = new Client
            {
                ClientID = 50,
                FirstName = "John",
                LastName = "Doe",
                DocType = "DNI",
                DocNumber = 34578800,
                BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2016, 09, 01),
                Email = "john.doe.test@hotmail.com",
                Password = PasswordUtilities.GenerateSHA256Hash("test", passwordSalt),
                PasswordSalt = passwordSalt
            };
            #endregion

            #region Dummy MassiveClientsListForIndexTests List
            var passwordSalt0 = PasswordUtilities.CreateSalt(16);
            var password0 = PasswordUtilities.GenerateSHA256Hash("12345", passwordSalt0);

            massiveClients = new List<Client>
            {
                new Client { ClientID=1, FirstName = "Alejandra", LastName = "Doe", DocType = "DNI", DocNumber = 34578800, BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "john.doe@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=2, FirstName = "Alberto", LastName = "Piqué", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1989, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "cristian.pique@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=3, FirstName = "Antonio", LastName = "Mosby", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1985, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "ted.mosby@gmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=1, FirstName = "Cristian", LastName = "Doe", DocType = "DNI", DocNumber = 34578800, BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "john.doe@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=2, FirstName = "Carlos", LastName = "Piqué", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1989, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "cristian.pique@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=3, FirstName = "Cecilia", LastName = "Mosby", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1985, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "ted.mosby@gmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=1, FirstName = "John", LastName = "Doe", DocType = "DNI", DocNumber = 34578800, BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "john.doe@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=2, FirstName = "Juan", LastName = "Piqué", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1989, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "cristian.pique@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=3, FirstName = "Ted", LastName = "Mosby", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1985, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "ted.mosby@gmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=1, FirstName = "Maria", LastName = "Doe", DocType = "DNI", DocNumber = 34578800, BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "john.doe@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=2, FirstName = "Martina", LastName = "Piqué", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1989, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "cristian.pique@hotmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 },

                new Client { ClientID=3, FirstName = "Zurdo", LastName = "Mosby", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1985, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "ted.mosby@gmail.com",
                Password = passwordSalt0, PasswordSalt = passwordSalt0 }
            };
            #endregion

            #region Repositories
            clientRepository = Mock.Create<IClientRepository>();
            #endregion

            #region Controller creation
            controller = new ClientsController(clientRepository);
            #endregion

            #region JustMock Arranges
            Mock.Arrange(() => clientRepository.GetClients())
                .Returns(clients)
                .MustBeCalled();

            Mock.Arrange(() => clientRepository.GetClientByID(CLIENT_ID_TO_USE))
                .Returns(clients.Where(c => c.ClientID == CLIENT_ID_TO_USE).FirstOrDefault())
                .MustBeCalled();

            Mock.Arrange(() => clientRepository.InsertClient(newClient))
                                               .DoInstead(() => clients.Add(newClient));

            Mock.Arrange(() => clientRepository.Save()).DoNothing();

            Mock.Arrange(() => clientRepository.DeleteClient(CLIENT_ID_TO_USE))
                .DoInstead(() => clients.Remove(clients.Where(c => c.ClientID == CLIENT_ID_TO_USE).FirstOrDefault()));

            Mock.Arrange(() => clientRepository.UpdateClient(new Client())).DoNothing();

            Mock.Arrange(() => clientRepository.IsEmailAlreadyInUse(newClient))
                                               .Returns(() => this.IsEmailAlreadyInUse(newClient, false))
                                               .MustBeCalled();
            #endregion
        }

        [TestMethod]
        public void Client_Index()
        {
            //Arrange //Act
            ViewResult viewResult = controller.Index(string.Empty,string.Empty, string.Empty, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Client>;

            //Assert
            Assert.AreEqual(clients.Count, model.Count());
            Assert.AreEqual("John", model.First().FirstName);
            Assert.IsTrue(model.Last().ClientID.GetType() == typeof(int));
        }

        [TestMethod]
        public void Client_IndexWithSort()
        {
            //Arrange //Act
            //Estas constantes van de la mano, y si se cambian requieren cambios en linea 389
            const string SORT_ORDER_DESC = "name_desc"; //change for name_asc if necessary
            const string SORT_MODE_DESC = "desc"; //change for asc if necessary

            Mock.Arrange(() => clientRepository.GetClients()) //Override
                .Returns(massiveClients)
                .MustBeCalled();

            ViewResult viewResult = controller.Index(SORT_ORDER_DESC, string.Empty, string.Empty, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Client>;

            if (SORT_MODE_DESC == "desc")
                massiveClients = massiveClients.OrderByDescending(c => c.FirstName).ToList();

            //Assert
            Assert.IsNotNull(model);
            Assert.IsTrue(massiveClients.FirstOrDefault().Equals(model.FirstOrDefault()));
            Assert.AreEqual(massiveClients.Take(model.Count()).Last().FirstName, model.Last().FirstName);
        }

        [TestMethod]
        public void Client_IndexWithSearch()
        {
            //Arrange //Act
            const string SEARCHSTRING = "Alejandra";
            var itemsFound = massiveClients.Where(c => string.Concat(c.FirstName, " ", c.LastName)
                                                 .ToLower()
                                                 .Contains(SEARCHSTRING.ToLower()));
            Mock.Arrange(() => clientRepository.GetClients()) //Override
                .Returns(massiveClients)
                .MustBeCalled();

            ViewResult viewResult = controller.Index(string.Empty, string.Empty, SEARCHSTRING, 1) as ViewResult;
            var model = viewResult.Model as IEnumerable<Client>;

            //Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(model.Except(model.Where(c => string.Concat(c.FirstName, " ", c.LastName).Contains(SEARCHSTRING))).Count(), 0);
        }

        [TestMethod]
        public void Client_Details()
        {
            var client = clients.Where(c => c.ClientID == CLIENT_ID_TO_USE).FirstOrDefault();

            ViewResult viewResult = controller.Details(CLIENT_ID_TO_USE) as ViewResult;
            var model = viewResult.Model as Client;

            Assert.IsNotNull(model);
            Assert.AreEqual(model.ClientID, CLIENT_ID_TO_USE);
            Assert.AreEqual(model.ClientID, client.ClientID);
            Assert.AreEqual(model.LastName, client.LastName);
        }

        [TestMethod]
        public void Client_Create()
        {
            int totalClientsBefore = clients.Count;

            ActionResult actionResult = controller.Create(newClient);

            //Assert
            Assert.AreNotEqual(totalClientsBefore, clients.Count);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Client_UpdateClient()
        {
            //Arrange 
            Client clientToUpdate = clients.FirstOrDefault();
            var originalName = clients.FirstOrDefault().FirstName;
            var originalLastName = clients.FirstOrDefault().LastName;

            Mock.Arrange(() => clientRepository.Save()) //Override Save arrange in Init()
                .DoInstead(() => { clientToUpdate.LastName = "Changed"; clientToUpdate.FirstName = "Changed!"; })
                .MustBeCalled();
            
            controller.Edit(clientToUpdate);
            
            //Assert
            Assert.IsFalse(clientToUpdate.FirstName == originalName);
            Assert.IsFalse(clientToUpdate.LastName == originalLastName);
        }

        [TestMethod]
        public void Client_HashPasswordAndCompareWithAttemptedPasswords()
        {
            var passSaltInDb = PasswordUtilities.CreateSalt(16);
            var passHashInDb = PasswordUtilities.GenerateSHA256Hash("TestingPassword", passSaltInDb);
            
            Assert.IsFalse(PasswordUtilities.Compare("TestingPa$$word", passHashInDb, passSaltInDb));
            Assert.IsFalse(PasswordUtilities.Compare("Testing Password", passHashInDb, passSaltInDb));
            Assert.IsFalse(PasswordUtilities.Compare(string.Empty, passHashInDb, passSaltInDb));
            Assert.IsFalse(PasswordUtilities.Compare(null, passHashInDb, passSaltInDb));
            Assert.IsTrue(PasswordUtilities.Compare("TestingPassword", passHashInDb, passSaltInDb));
        }

        [TestMethod]
        public void Client_EmailAlreadyInUseForCreate()
        {
            //Arrange 
            int totalClientsBefore = clients.Count;
            newClient.Email = clients.FirstOrDefault().Email; //Email already in use

            //Act
            controller.Create(newClient);

            //Assert
            Assert.IsTrue(totalClientsBefore  == clients.Count());
        }

        [TestMethod]
        public void Client_EmailNotInUseForCreate()
        {
            //Arrange 
            int totalClientsBefore = clients.Count;

            //Act
            controller.Create(newClient);

            //Assert
            Assert.IsTrue(totalClientsBefore < clients.Count());
        }

        [TestMethod]
        public void Client_EmailAlreadyInUseForEdit()
        {
            //Arrange //Act
            var client = clients.FirstOrDefault();
            var originalName = clients.FirstOrDefault().FirstName;
            var originalLastName = clients.FirstOrDefault().LastName;
            client.Email = clients.Last().Email; //Email already in use
            Mock.Arrange(() => clientRepository.Save()) //Override Save arrange in Init()
                .DoInstead(() => { client.LastName = "Changed"; client.FirstName = "Changed!"; })
                .MustBeCalled();
            Mock.Arrange(() => clientRepository.IsEmailAlreadyInUse(client)) //Override
                .Returns(() => this.IsEmailAlreadyInUse(client, true))
                .MustBeCalled();

            controller.Edit(client);

            Assert.IsTrue(client.FirstName == originalName);
            Assert.IsTrue(client.LastName == originalLastName);
        }

        [TestMethod]
        public void Client_EmailNotInUseForEdit()
        {
            //Arrange //Act
            var client = clients.FirstOrDefault();
            var originalName = clients.FirstOrDefault().FirstName;
            var originalLastName = clients.FirstOrDefault().LastName;
            Mock.Arrange(() => clientRepository.IsEmailAlreadyInUse(client)) //Override
                .Returns(() => this.IsEmailAlreadyInUse(client, true))
                .MustBeCalled();
            Mock.Arrange(() => clientRepository.Save()) //Override
                .DoInstead(() => { client.LastName = "Changed"; client.FirstName = "Changed!"; })
                .MustBeCalled();

            controller.Edit(client);

            Assert.IsFalse(client.FirstName == originalName);
            Assert.IsFalse(client.LastName == originalLastName);
        }

        [TestMethod]
        public void Activity_Edit_HttpGet()
        {
            ActionResult actionResult = controller.Edit(CLIENT_ID_TO_USE);
            var model = (actionResult as ViewResult).Model;

            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(actionResult, typeof(ViewResult));
        }

        [TestMethod]
        public void Activity_Edit_HttpPost()
        {
            var originalActivity = clients.FirstOrDefault();
            var originalName = clients.FirstOrDefault().FirstName;
            var originalDesc = clients.FirstOrDefault().LastName;
            Mock.Arrange(() => clientRepository.Save()).DoInstead(() => { originalActivity.FirstName = "Changed!"; originalActivity.LastName = "Desc changed!"; });

            ActionResult actionResult = controller.Edit(newClient) as ActionResult;

            Assert.AreNotEqual(originalActivity.FirstName, originalName);
            Assert.AreNotEqual(originalActivity.LastName, originalDesc);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        }


        [TestMethod]
        public void Client_Edit_HttpGet_HttpNotFound()
        {
            Mock.Arrange(() => clientRepository.GetClientByID(CLIENT_ID_NOT_FOUND))
                                                   .Returns(clients.Where(c => c.ClientID == CLIENT_ID_NOT_FOUND).FirstOrDefault());

            ActionResult actionResult = controller.Edit(CLIENT_ID_NOT_FOUND); //A very high AssistanceID

            var result = actionResult as HttpNotFoundResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(actionResult, typeof(HttpNotFoundResult));
            Assert.IsNotNull(result.GetType().GetProperty("StatusDescription"), null);
            Assert.AreEqual(result.StatusCode, 404);
        }

        [TestMethod]
        public void Client_Delete()
        {
            //Arrange 
            int totalClientsBefore = clients.Count;
            var idToDelete = clients.FirstOrDefault().ClientID;

            controller.DeleteConfirmed(clients.FirstOrDefault().ClientID);

            //Assert
            Assert.IsTrue(totalClientsBefore > clients.Count);
            Assert.IsFalse(clients.Any(c => c.ClientID == idToDelete));
        }

        [TestMethod]
        public void TestRandomPassword()
        {
            var pass10 = PasswordUtilities.RandomPassword(10);
            var pass8 = PasswordUtilities.RandomPassword(8);
            var pass15 = PasswordUtilities.RandomPassword(15);
            var invalidPassChar = PasswordUtilities.RandomPassword(15); invalidPassChar += "ñ";
            var invalidPassLength = PasswordUtilities.RandomPassword(15); invalidPassLength += "ñ";

            string pattern = @"^[a-z0-9]+$";
            Regex r = new Regex(pattern);
            Assert.IsTrue(r.Match(pass8).Success);
            Assert.IsTrue(r.Match(pass10).Success);
            Assert.IsTrue(r.Match(pass15).Success);
            Assert.AreEqual(pass8.Length, 8);
            Assert.AreEqual(pass10.Length, 10);
            Assert.AreEqual(pass15.Length, 15);
            Assert.IsFalse(pass8.Contains("ñ"));
            Assert.IsFalse(pass10.Contains("ñ"));
            Assert.IsFalse(pass15.Contains("ñ"));
            Assert.AreNotEqual(invalidPassLength.Length, 15);
            Assert.IsFalse(r.Match(invalidPassChar).Success);

        }

        #region Private Methods
        private bool IsEmailAlreadyInUse(Client client, bool IsEditTheCaller)
        {
            bool isEditTheCaller = IsEditTheCaller;

            if (isEditTheCaller) //Si viene de Edit, debo permitirle guardar el email que ya tenía anteriormente
            {
                return clients.Where(c => c.ClientID != client.ClientID)
                              .Any(c => c.Email.ToLower() == client.Email.ToLower());
            }
            else
            {
                return clients.Any(c => c.Email.ToLower() == client.Email.ToLower());
            }
        }
        #endregion
    }
}
