using ProyectoFinal.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models
{
    public class GymContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public GymContext() : base("GymContext")
        { }

        #region DbSets
        public DbSet<Activity> Activities { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Assistance> Assistances { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<MedicalRecord> MedicalRecords { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<PaymentTypePrice> PaymentTypePrices { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Routine> Routines { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }
        #endregion

      //  public System.Data.Entity.DbSet<ProyectoFinal.Models.OldGroup> Groups { get; set; }

        public System.Data.Entity.DbSet<ProyectoFinal.Models.Registration> Registrations { get; set; }
    }

    public class GymInitializer : DropCreateDatabaseAlways<GymContext>
    {
        protected override void Seed(GymContext context)
        {
            //test
            #region Clients
            var passwordSalt1 = PasswordUtilities.CreateSalt(16);
            var password1 = PasswordUtilities.GenerateSHA256Hash("12345", passwordSalt1);
            var passwordSalt2 = PasswordUtilities.CreateSalt(16);
            var password2 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt2);
            var passwordSalt3 = PasswordUtilities.CreateSalt(16);
            var password3 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt3);
            var passwordSalt4 = PasswordUtilities.CreateSalt(16);
            var password4 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt4);
            var passwordSalt5 = PasswordUtilities.CreateSalt(16);
            var password5 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt5);
            var passwordSalt6 = PasswordUtilities.CreateSalt(16);
            var password6 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt6);
            var passwordSalt7 = PasswordUtilities.CreateSalt(16);
            var password7 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt7);
            var passwordSalt8 = PasswordUtilities.CreateSalt(16);
            var password8 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt8);
            var passwordSalt9 = PasswordUtilities.CreateSalt(16);
            var password9 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt9);
            var passwordSalt10 = PasswordUtilities.CreateSalt(16);
            var password10 = PasswordUtilities.GenerateSHA256Hash("335588", passwordSalt10);
            var passwordSalt11 = PasswordUtilities.CreateSalt(16);
            var password11 = PasswordUtilities.GenerateSHA256Hash("234566", passwordSalt11);
            var passwordSalt12 = PasswordUtilities.CreateSalt(16);
            var password12 = PasswordUtilities.GenerateSHA256Hash("234566", passwordSalt12);
            var passwordSalt13 = PasswordUtilities.CreateSalt(16);
            var password13 = PasswordUtilities.GenerateSHA256Hash("234566", passwordSalt13);
            var passwordSalt14 = PasswordUtilities.CreateSalt(16);
            var password14 = PasswordUtilities.GenerateSHA256Hash("234566", passwordSalt14);

            var clients = new List<Client>
            {
                new Client { FirstName = "John", LastName = "Doe", DocType = "DNI", DocNumber = 34578800, BirthDate = new DateTime(1990, 12, 31),
                DateFrom = new DateTime(2008, 09, 01), Email = "john.doe@hotmail.com",
                Password = password1, PasswordSalt = passwordSalt1, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "Cristian", LastName = "Piqué", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1989, 12, 31),
                DateFrom = new DateTime(2008, 09, 01), Email = "cristian.pique@hotmail.com",
                Password = password2, PasswordSalt = passwordSalt2, Role = Catalog.Roles.Admin, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "José", LastName = "Pérez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1982, 12, 31),
                DateFrom = new DateTime(2014, 09, 01), Email = "jose.perez@hotmail.com",
                Password = password3, PasswordSalt = passwordSalt3, Role = Catalog.Roles.Instructor, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "Ray", LastName = "Allen", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1992, 12, 31),
                DateFrom = new DateTime(2014, 09, 01), Email = "ray.allan@hotmail.com",
                Password = password4, PasswordSalt = passwordSalt4, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "Enzo", LastName = "Gutiérrez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1991, 12, 31),
                DateFrom = new DateTime(2014, 09, 01), Email = "enzog@gmail.com",
                Password = password5, PasswordSalt = passwordSalt5, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "Juana", LastName = "Pérez", DocType = "DNI", DocNumber = 33123654, BirthDate = new DateTime(1979, 08, 11),
                DateFrom = new DateTime(2015, 09, 01), Email = "juana.perez@hotmail.com",
                Password = password6, PasswordSalt = passwordSalt6, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Mujer },

                new Client { FirstName = "Carolina", LastName = "García", DocType = "DNI", DocNumber = 12345678, BirthDate = new DateTime(1971, 11, 15),
                DateFrom = new DateTime(2015, 09, 01), Email = "caro.garcia@hotmail.com",
                Password = password7, PasswordSalt = passwordSalt7, Role = Catalog.Roles.Instructor, Sexo = Catalog.Genre.Mujer },

                new Client { FirstName = "Martina", LastName = "Núñez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1981, 02, 01),
                DateFrom = new DateTime(2015, 09, 01), Email = "martinanunez@yahoo.com",
                Password = password8, PasswordSalt = passwordSalt8, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Mujer },

                new Client { FirstName = "Sol", LastName = "Rodríguez", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1991, 12, 31),
                DateFrom = new DateTime(2015, 09, 01), Email = "sol.rodriguez@outlook.com",
                Password = password9, PasswordSalt = passwordSalt9, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Mujer },

                new Client { FirstName = "José", LastName = "García", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1986, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "joseg@hotmail.com",
                Password = password10, PasswordSalt = passwordSalt10, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },
               
                new Client { FirstName = "Administrador", LastName = "Administrador", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1986, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "administrador@hotmail.com",
                Password = password11, PasswordSalt = passwordSalt11, Role = Catalog.Roles.Admin, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "Instructor", LastName = "Instructor", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1986, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "instructor@hotmail.com",
                Password = password12, PasswordSalt = passwordSalt12, Role = Catalog.Roles.Instructor, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "Cliente", LastName = "Cliente", DocType = "DNI", DocNumber = 34578644, BirthDate = new DateTime(1986, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "cliente@hotmail.com",
                Password = password13, PasswordSalt = passwordSalt13, Role = Catalog.Roles.Client, Sexo = Catalog.Genre.Hombre },

                new Client { FirstName = "Fabricio", LastName = "Montes", DocType = "DNI", DocNumber = 34965835, BirthDate = new DateTime(1986, 12, 31),
                DateFrom = new DateTime(2016, 09, 01), Email = "fabri_3089@hotmail.com",
                Password = password14, PasswordSalt = passwordSalt14, Role = Catalog.Roles.Admin, Sexo = Catalog.Genre.Hombre }
            };

            clients.ForEach(c => context.Clients.Add(c));
            context.SaveChanges();
            #endregion

            #region Routines
            var routines = new List<Routine>
            {
                new Routine { ClientID = 1, NameFile="CristianPique v1.0", Description = "Rutina personalizada", Files = new List<File>(), CreationDate = DateTime.Now, DaysInWeek=5,
                              Level =Catalog.LevelRoutine.Medium, Status= Catalog.Status.Active },
                new Routine { ClientID = 1, NameFile="RutinaBegginers", Description = "Rutina nivel princiante", Files = new List<File>(), CreationDate = DateTime.Now, DaysInWeek=3,
                              Level=Catalog.LevelRoutine.Begginer, Status= Catalog.Status.Active},
                new Routine { ClientID = 1, NameFile="Cardio rutina", Description = "Rutina intensiva cardio", Files = new List<File>(), CreationDate = DateTime.Now, DaysInWeek=4,
                              Level=Catalog.LevelRoutine.Medium, Status= Catalog.Status.Active},
                new Routine { ClientID = 1, NameFile="AbdominalesRutina", Description = "Rutina abdominales", Files = new List<File>(), CreationDate = DateTime.Now, DaysInWeek=4,
                              Level=Catalog.LevelRoutine.Expert, Status= Catalog.Status.Active}
            };

            routines.ForEach(r => context.Routines.Add(r));
            context.SaveChanges();
            #endregion

            #region MedicalRecords
            var medicalRecords = new List<MedicalRecord>
            {
                new MedicalRecord { ClientID = 1, Age = 26, Gender = 'M', Heigth = 1.81, Weight = 75 },
            };

            medicalRecords.ForEach(m => context.MedicalRecords.Add(m));
            context.SaveChanges();
            #endregion

            #region Activities
            var clientsForActivities = new List<Client>();
            clientsForActivities.Add(clients.Where(x => x.ClientID == 1).FirstOrDefault());
            clientsForActivities.Add(clients.Where(x => x.ClientID == 1).FirstOrDefault());

            var activities = new List<Activity>
            {
                new Activity { Name = "Gimnasio", Description = "Gimnasio, pesas, bicicletas, máquinas para correr"},
                new Activity { Name = "Pilates", Description = "Sistema de entrenamiento físico y mental"},
                new Activity { Name = "Boxeo", Description = "Deporte de combate"}
            };

            activities.ForEach(a => context.Activities.Add(a));
            context.SaveChanges();
            #endregion

      
            #region Files
            var files = new List<File>
            {
                new File { RoutineID=1, ExerciseName="Pecho inclinado", MuscleName="Pecho", Peso="20", Repetitions="3x10", Day="Lunes" },
                new File { RoutineID=1, ExerciseName="Pecho con mancuernas", MuscleName="Pecho", Peso="15", Repetitions="3x12", Day="Martes" },
                new File { RoutineID=1, ExerciseName="Pecho con mancuernas", MuscleName="Pecho", Peso="20", Repetitions="3x8", Day="Martes" },
                new File { RoutineID=1, ExerciseName="Bíceps con mancuernas", MuscleName="Bíceps", Peso="8", Repetitions="3x12", Day="Miércoles" },
                new File { RoutineID=1, ExerciseName="Bíceps con mancuernas", MuscleName="Bíceps", Peso="10", Repetitions="3x12", Day="Jueves" },
                new File { RoutineID=1, ExerciseName="Francés", MuscleName="Tríceps", Peso="8", Repetitions="3x12", Day="Jueves" },
                new File { RoutineID=1, ExerciseName="Tríceps con mancuernas", MuscleName="Tríceps", Peso="8", Repetitions="3x12", Day="Viernes" },
                new File { RoutineID=1, ExerciseName="Press militar", MuscleName="Hombros", Peso="8", Repetitions="3x12", Day="Viernes" },
                new File { RoutineID=2, ExerciseName="Pecho inclinado", MuscleName="Pecho", Peso="20", Repetitions="3x10", Day="Lunes" },
                new File { RoutineID=2, ExerciseName="Pecho con mancuernas", MuscleName="Pecho", Peso="15", Repetitions="3x12", Day="Martes" },
                new File { RoutineID=2, ExerciseName="Pecho con mancuernas", MuscleName="Pecho", Peso="20", Repetitions="3x8", Day="Martes" },
                new File { RoutineID=2, ExerciseName="Bíceps con mancuernas", MuscleName="Bíceps", Peso="8", Repetitions="3x12", Day="Miércoles" },
                new File { RoutineID=2, ExerciseName="Bíceps con mancuernas", MuscleName="Bíceps", Peso="10", Repetitions="3x12", Day="Jueves" },
                new File { RoutineID=2, ExerciseName="Francés", MuscleName="Tríceps", Peso="8", Repetitions="3x12", Day="Jueves" },
                new File { RoutineID=2, ExerciseName="Tríceps con mancuernas", MuscleName="Tríceps", Peso="8", Repetitions="3x12", Day="Viernes" },
                new File { RoutineID=2, ExerciseName="Press militar", MuscleName="Hombros", Peso="8", Repetitions="3x12", Day="Viernes" },
                new File { RoutineID=3, ExerciseName="Biclicleta", MuscleName="Piernas", Peso="-", Repetitions="20 min", Day="Lunes" },
                new File { RoutineID=3, ExerciseName="Cinta", MuscleName="Piernas", Peso="15", Repetitions="3x12", Day="Lunes" },
                new File { RoutineID=3, ExerciseName="Pecho con mancuernas", MuscleName="Pecho", Peso="20", Repetitions="3x8", Day="Martes" },
                new File { RoutineID=3, ExerciseName="Abdominales oblicuos", MuscleName="Abdominales", Peso="-", Repetitions="3x20", Day="Martes" },
                new File { RoutineID=3, ExerciseName="Abdominales bajos", MuscleName="Abdominales", Peso="-", Repetitions="3x30", Day="Míércoles" },
                new File { RoutineID=3, ExerciseName="Saltar soga", MuscleName="Piernas", Peso="-", Repetitions="15 min", Day="Míércoles" },
                new File { RoutineID=3, ExerciseName="Tríceps con mancuernas", MuscleName="Tríceps", Peso="8", Repetitions="3x12", Day="Jueves" },
                new File { RoutineID=3, ExerciseName="Biclicleta", MuscleName="Pecho", Peso="-", Repetitions="20 min", Day="Lunes" },
                new File { RoutineID=3, ExerciseName="Biclicleta", MuscleName="Piernas", Peso="-", Repetitions="20 min", Day="Lunes" },
                new File { RoutineID=3, ExerciseName="Cinta", MuscleName="Piernas", Peso="15", Repetitions="3x12", Day="Lunes" },
            };

            files.ForEach(f => context.Files.Add(f));
            context.SaveChanges();
            #endregion

            #region PaymentType
            var paymentType = new List<PaymentType>
            {
                new PaymentType { Description = "Gimnasio mensual", Status = Catalog.Status.Active, ActivityID = 1, DurationInMonths = 1 },
                new PaymentType { Description = "Gimnasio anual", Status = Catalog.Status.Active, ActivityID = 1, DurationInMonths = 12 },
                new PaymentType { Description = "Pilates mensual", Status = Catalog.Status.Active, ActivityID = 2, DurationInMonths = 1 },
                 new PaymentType { Description = "Pilates anual", Status = Catalog.Status.Active, ActivityID = 2, DurationInMonths = 12 },
                new PaymentType { Description = "Boxeo mensual", Status = Catalog.Status.Active, ActivityID = 3, DurationInMonths = 1 },
                new PaymentType { Description = "Boxeo anual", Status = Catalog.Status.Active, ActivityID = 3, DurationInMonths = 12 },
            };

            paymentType.ForEach(p => context.PaymentTypes.Add(p));
            context.SaveChanges();
            #endregion

            #region Payment
            var payments = new List<Payment>
            {
                new Payment { ClientID = 1, PaymentTypeID = 1, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2017,12,12), CreationDate = DateTime.Now },
                new Payment { ClientID = 2, PaymentTypeID = 2, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2017,12,12), CreationDate = DateTime.Now },

                new Payment { ClientID = 4, PaymentTypeID = 1, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2016,12,12), CreationDate = DateTime.Now },
                new Payment { ClientID = 4, PaymentTypeID = 2, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2016,12,12), CreationDate = DateTime.Now },

                new Payment { ClientID = 5, PaymentTypeID = 3, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2016,12,12), CreationDate = DateTime.Now },
                new Payment { ClientID = 5, PaymentTypeID = 3, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2016,12,12), CreationDate = DateTime.Now },

                new Payment { ClientID = 6, PaymentTypeID = 4, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2016,12,12), CreationDate = DateTime.Now },
                new Payment { ClientID = 6, PaymentTypeID = 4, Status = Catalog.Status.Active, ExpirationDate = new DateTime(2016,12,12), CreationDate = DateTime.Now }
            };

            payments.ForEach(p => context.Payments.Add(p));
            context.SaveChanges();
            #endregion

            #region PaymentTypePrices
            var paymentTypePrices = new List<PaymentTypePrice>
            {
                new PaymentTypePrice { DateFrom = new DateTime(2016,01,01), PaymentTypeID = 1, Price = 200 },
                new PaymentTypePrice { DateFrom = new DateTime(2016,05,01), PaymentTypeID = 1, Price = 300 },
                new PaymentTypePrice { DateFrom = new DateTime(2016,01,01), PaymentTypeID = 1, Price = 2000 }
            };

            paymentTypePrices.ForEach(ptp => context.PaymentTypePrices.Add(ptp));
            context.SaveChanges();
            #endregion

            #region Suppliers
            var suppliers = new List<Supplier>
            {
                new Supplier { Address = "Entre Rios 2456", BusinessName = "EuroGym", City="Rosario", Country="Argentina", Email="eurogym@gmail.com", PhoneNumber="(341) 42145580",
                               PostalCode = 2000, WebSite = "http://www.eurogym.com.ar"},
                new Supplier { Address = "Santa Fé Av 1052", BusinessName = "Total Gym", City="Buenos Aires", Country="Argentina", Email="totalgym@productos.com", PhoneNumber="(011) 5445-7189",
                               PostalCode = 1059, WebSite = "http://www.totalgym.com.ar"},
                new Supplier { Address = "V. Cardoso 1401", BusinessName = "e-punto Fitness", City="Ramos Mejia", Country="Argentina", Email="info@e-puntofitness.com.ar", PhoneNumber="(011) 4654-2160",
                               PostalCode = 1059, WebSite = "http://www.e-puntofitness.com.ar/"},
                new Supplier { Address = "La Paz 138", BusinessName = "Distribuidora Boom S.R.L", City="Rosario", Country="Argentina", Email="info@boomventas.com.ar", PhoneNumber="(341) 1564895212",
                               PostalCode = 2000, WebSite = "http://www.boomsrl.com.ar/"},
            };

            suppliers.ForEach(s => context.Suppliers.Add(s));
            context.SaveChanges();
            #endregion

            #region Products
            var products = new List<Product>
            {
                new Product { Name="Cama de Pilates", Description="Cama de pilates + tabla de salto + tabla de extensión + box Garantía 1 año", Price = 8000, PurchaseDate = new DateTime(2014,05,02),
                              Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=3  },
                new Product { Name="Sillón de cuadriceps", Description="Sillon de cuadriceps c/75kgrs.Linea Exclusive", Price = 25560, PurchaseDate = new DateTime(2015,08,02),
                              Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=3  },
                new Product { Name="Bicicleta Indoor", Description="Transmisión a cadena Volante de inercia de 20Kg. balanceado dinámicamente cromado y pintado en el centro. Asiento prostático. Manubrio regulable en altura.",
                              Price = 6099, PurchaseDate = new DateTime(2015,07,22), Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=3  },
                new Product { Name="Cinta Profesional", Description="Motor 3hp corriente alterna uso industrial 24hs - Velocidad de 0,8 a 16 km/h programable hasta 20km/h- Banda de doble tela antideslizante y antiestática con carbono .",
                              Price = 37550, PurchaseDate = new DateTime(2014,05,02), Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=3  },
                new Product { Name="Complejo Multiestación", Description="El complejo de cuatro estaciones de uso profesional esta compuesto por una Camilla femoral y sillon de cuadriceps con una carga de 50 Kg mas una polea simple alta y baja de 10 regulaciones y 50 Kg de carga mas una pectoralera con 50 Kg de carga y una Dorsalera con 75 kg de carga. ",
                              Price = 97500, PurchaseDate = new DateTime(2014,05,02), Status = Utils.Catalog.ProductStatus.Deteriorated, Type = Utils.Catalog.ProductType.Machine, SupplierID=1  },
                new Product { Name="Polea simple regulable 12 niveles", Description="Polea simple con lingotera de 75 kilos y 12 regulaciones línea profesional Exclusive.",
                              Price = 6512, PurchaseDate = new DateTime(2014,05,02), Status = Utils.Catalog.ProductStatus.Broken, Type = Utils.Catalog.ProductType.Machine, SupplierID=1  },
                new Product { Name="Pectoralera c/75kgrs", Description="Pectoralera mariposa con lingotera de 75 kilos línea profesional Exclusive",
                              Price = 5300, PurchaseDate = new DateTime(2014,05,02), Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=2  },
                new Product { Name="Remo Bajo", Description="Press de pecho con lingotera de 100 kilos línea profesional Exclusive.",
                              Price = 6100, PurchaseDate = new DateTime(2014,05,02), Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=4  },
                new Product { Name="Agua Villavicencio 500ml", Description="Botella de agua mineral Villavicencio", Price = 1050, PurchaseDate = new DateTime(2016,09,02),
                              Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Article, SupplierID=4  },
                new Product { Name="Gatorade 750ml", Description="Gatorade 750ml Naranja", Price = 20, PurchaseDate = new DateTime(2016,09,02),
                              Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Article, SupplierID=4  },
                new Product { Name="Banco Pecho Modulo", Description="Banco regulable que permite ejercicios con barra y camilla de cuadriceps y femoral a discos.",
                              Price = 2300, PurchaseDate = new DateTime(2016,09,02), Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=1  },
                new Product { Name="Banco Pecho Regulable", Description="Banco de pecho regulable standard", Price = 3100, PurchaseDate = new DateTime(2016,09,02),
                              Status = Utils.Catalog.ProductStatus.Ok, Type = Utils.Catalog.ProductType.Machine, SupplierID=2  },
            };

            products.ForEach(p => context.Products.Add(p));
            context.SaveChanges();
            #endregion

            #region Assistances
            var assistances = new List<Assistance>
            {
                new Assistance { assistanceDate = new DateTime(2016,01,01), ClientID = 1 },
                new Assistance { assistanceDate = new DateTime(2015,01,01), ClientID = 1 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 1 },
                new Assistance { assistanceDate = new DateTime(2016,01,01), ClientID = 2 },
                new Assistance { assistanceDate = new DateTime(2015,01,01), ClientID = 2 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 2 },
                new Assistance { assistanceDate = new DateTime(2016,01,01), ClientID = 3 },
                new Assistance { assistanceDate = new DateTime(2015,01,01), ClientID = 3 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 3 },
                new Assistance { assistanceDate = new DateTime(2015,01,01), ClientID = 4 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 4 },
                new Assistance { assistanceDate = new DateTime(2015,01,01), ClientID = 5 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 5 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 6 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 6 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 6 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 8 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 7 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 7 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 7 },
                new Assistance { assistanceDate = new DateTime(2016,02,01), ClientID = 7 }
            };

            assistances.ForEach(a => context.Assistances.Add(a));
            context.SaveChanges();
            #endregion

            #region Groups
            var groups = new List<Group>
            {
                new Models.Group {Name="CrossFit", Description="Entrenamiento con movimientos funcionales, ejecutados a alta intensidad.", Level=Catalog.LevelGroup.Begginer, Quota=20, Amount=0, Day="Lunes", HourFrom=13, HourTo=14, ActivityID=1 },
                new Models.Group {Name="CrossFit 2", Description="Entrenamiento con movimientos funcionales, ejecutados a alta intensidad.", Level=Catalog.LevelGroup.Medium, Quota=20, Amount=0, Day="Lunes", HourFrom=14, HourTo=15, ActivityID=1 },
                new Models.Group {Name="Pilates", Description="Sistema de entrenamiento físico y mental", Level=Catalog.LevelGroup.Begginer, Quota=20, Amount=0, Day="Lunes", HourFrom=13, HourTo=14, ActivityID=2 },
                new Models.Group {Name="Boxeo", Description="Deporte de combate", Level=Catalog.LevelGroup.Begginer, Quota=0, Amount=10, Day="Martes", HourFrom=13, HourTo=14, ActivityID=3 }
            }; groups.ForEach(a => context.Groups.Add(a));
            context.SaveChanges();
            #endregion

             #region Registrations
             var registrations = new List<Registration>
             {
                 new Models.Registration {CreationDate=  new DateTime(2017,12,12), Status=Catalog.Status.Active, ClientID=1, GroupID=1 },
                 //new Models.Registration {CreationDate=  new DateTime(2017,12,12), Status=Catalog.Status.Active, ClientID=1, GroupID=2 }
             };
             registrations.ForEach(a => context.Registrations.Add(a));
             context.SaveChanges();
            #endregion

            #region updateAmountAndQuota
           var inscripciones = context.Registrations.ToList();
            foreach (var item in inscripciones)
            {
                var clase = context.Groups.Where(g => g.GroupID == item.GroupID).FirstOrDefault();
                if(clase!=null)
                {
                    clase.Amount += 1;
                    clase.Quota -= 1;
                }
            }
            context.SaveChanges();
            #endregion
            #region updatePaymentStatus
            var payment = context.Payments.ToList();
            foreach (var item in payment)
            {
               if(item.ExpirationDate<= DateTime.Now)
                {
                    item.Status = Catalog.Status.Inactive;
                }
            }
            context.SaveChanges();
            #endregion

        }

    }
}