using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProyectoFinal.Models;
using ProyectoFinal.Models.Repositories;
using ProyectoFinal.Controllers;
using Telerik.JustMock;
using ProyectoFinal.Utils;
using System.Web;
using System.Linq;
using System.Web.Mvc;

namespace ProyectoFinal.Tests
{
    /// <summary>
    /// Descripción resumida de DashBoardControllerTest
    /// </summary>
    [TestClass]
    public class DashBoardControllerTest
    {
        List<Client> clients;
        List<MedicalRecord> medicalRecords;
        IClientRepository clientRepository;
        IMedicalRecordRepository medicalRecordRepository;
        DashBoardController controller;
        public const int CLIENT_ID_TO_USE = 1;

        HttpContextBase context;
        HttpRequestBase request;
        HttpResponseBase response;
        HttpSessionStateBase session;
        HttpServerUtilityBase server;

        [TestInitialize]
        public void Init()
        {
            #region Dummy Lists
            var passwordSalt1 = PasswordUtilities.CreateSalt(16);
            var password1 = PasswordUtilities.GenerateSHA256Hash("12345", passwordSalt1);

            clients = new List<Client>
            {
                new Client { ClientID = 1, FirstName = "John", LastName = "Doe", DocType = "DNI", DocNumber = 34578800, BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2008, 09, 01), Email = "john.doe@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },

                new Client { ClientID = 2, FirstName = "Cristian", LastName = "Piqué", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1989, 12, 31),
                DateFrom = new DateTime(2008, 09, 01), Email = "cristian.pique@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Admin, Sexo = Catalog.Genre.Hombre },

                new Client { ClientID = 3, FirstName = "José", LastName = "Pérez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1982, 12, 31),
                DateFrom = new DateTime(2014, 09, 01), Email = "jose.perez@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Instructor, Sexo = Catalog.Genre.Hombre },

                new Client { ClientID = 4, FirstName = "Ray", LastName = "Allen", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1992, 12, 31),
                DateFrom = new DateTime(2014, 09, 01), Email = "ray.allan@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },

                new Client { ClientID = 5, FirstName = "Enzo", LastName = "Gutiérrez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1991, 12, 31),
                DateFrom = new DateTime(2014, 09, 01), Email = "enzog@gmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },

                new Client { ClientID = 6, FirstName = "Juana", LastName = "Pérez", DocType = "DNI", DocNumber = 33123654, BirthDate = new DateTime(1979, 08, 11),
                DateFrom = new DateTime(2015, 09, 01), Email = "juana.perez@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Mujer },

                new Client { ClientID = 7, FirstName = "Carolina", LastName = "García", DocType = "DNI", DocNumber = 12345678, BirthDate = new DateTime(1971, 11, 15),
                DateFrom = new DateTime(2015, 09, 01), Email = "caro.garcia@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Instructor, Sexo = Catalog.Genre.Mujer },

                new Client { ClientID = 8, FirstName = "Martina", LastName = "Núñez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1981, 02, 01),
                DateFrom = new DateTime(2015, 09, 01), Email = "martinanunez@yahoo.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Mujer },

                new Client { ClientID = 9, FirstName = "Sol", LastName = "Rodríguez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1991, 12, 31),
                DateFrom = new DateTime(2015, 09, 01), Email = "sol.rodriguez@outlook.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Mujer },

                new Client { ClientID = 10, FirstName = "José", LastName = "García", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1986, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "joseg@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre }
            };

            medicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord { ClientID = 1, Age = 26, Gender = 'M', Heigth = 1.81, Weight = 77 },
                new MedicalRecord { ClientID = 2, Age = 33, Gender = 'F', Heigth = 1.81, Weight = 75 },
                new MedicalRecord { ClientID = 3, Age = 28, Gender = 'F', Heigth = 1.81, Weight = 75 },
                new MedicalRecord { ClientID = 4, Age = 23, Gender = 'M', Heigth = 1.81, Weight = 79 },
                new MedicalRecord { ClientID = 5, Age = 26, Gender = 'M', Heigth = 1.81, Weight = 75 },
                new MedicalRecord { ClientID = 6, Age = 26, Gender = 'F', Heigth = 1.81, Weight = 75 },
                new MedicalRecord { ClientID = 7, Age = 25, Gender = 'M', Heigth = 1.81, Weight = 88 },
                new MedicalRecord { ClientID = 8, Age = 26, Gender = 'M', Heigth = 1.81, Weight = 75 },
                new MedicalRecord { ClientID = 10, Age = 18, Gender = 'M', Heigth = 1.81, Weight = 75 },
                new MedicalRecord { ClientID = 4, Age = 26, Gender = 'M', Heigth = 1.81, Weight = 75 },
            };
            #endregion

            #region Repositories
            clientRepository = Mock.Create<IClientRepository>();
            medicalRecordRepository = Mock.Create<IMedicalRecordRepository>();
            context = Mock.Create<HttpContextBase>();
            request = Mock.Create<HttpRequestBase>();
            response = Mock.Create<HttpResponseBase>();
            session = Mock.Create<HttpSessionStateBase>();
            server = Mock.Create<HttpServerUtilityBase>();
            #endregion

            #region Controller
            controller = new DashBoardController(clientRepository, medicalRecordRepository);
            #endregion

            #region Fake Context
            Mock.Arrange(() => clientRepository
                                    .GetClientByID(CLIENT_ID_TO_USE))
                                    .Returns(clients.Where(c => c.ClientID == CLIENT_ID_TO_USE)
                                    .FirstOrDefault());

            Mock.Arrange(() => context.Request).Returns(request);
            Mock.Arrange(() => context.Response).Returns(response);
            Mock.Arrange(() => context.Session).Returns(session);
            Mock.Arrange(() => context.Server).Returns(server);
            Mock.Arrange(() => session["UserData"]).Returns(clients.Where(c => c.ClientID == CLIENT_ID_TO_USE));
            #endregion

            #region JustMock
            #endregion
        }

        //Usa Session, lo que dificulta los test unitarios
        //[TestMethod]
        public void DashBoard_Index()
        {
            throw new NotImplementedException();
        }

    }

    
    
}
