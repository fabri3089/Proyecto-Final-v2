using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Services;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace ProyectoFinal.Tests
{
    /// <summary>
    /// Descripción resumida de HomeControllerTest
    /// </summary>
    [TestClass]
    public class HomeControllerTest
    {
        public HomeControllerTest()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtiene o establece el contexto de las pruebas que proporciona
        ///información y funcionalidad para la serie de pruebas actual.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Atributos de prueba adicionales
        //
        // Puede usar los siguientes atributos adicionales conforme escribe las pruebas:
        //
        // Use ClassInitialize para ejecutar el código antes de ejecutar la primera prueba en la clase
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup para ejecutar el código una vez ejecutadas todas las pruebas en una clase
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Usar TestInitialize para ejecutar el código antes de ejecutar cada prueba 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup para ejecutar el código una vez ejecutadas todas las pruebas
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void TestEmail()
        {
            Mailing mailing = new Mailing();
            string result = mailing.Execute();
            Assert.AreEqual(result, string.Empty);
        }

        [TestMethod]
        public void ExecuteHTMLBody()
        {
            Mailing mailing = new Mailing();
            string result = mailing.ExecuteHTMLBody(@"C:\Users\Usuario\Desktop\ProyectoFinal\ProyectoFinal\Templates\EmailTemplate.html");
            Assert.AreEqual(result, string.Empty);
        }
    }
}
