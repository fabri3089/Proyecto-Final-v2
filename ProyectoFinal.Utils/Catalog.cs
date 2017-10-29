using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Utils
{
    public class Catalog
    {
        public enum Status { Active = 1, Inactive }
        public enum Roles { Admin = 1, Instructor, Client }
        public enum LevelRoutine { Begginer = 1, Medium, Advanced, Expert}
        public enum LevelGroup { Begginer = 1, Medium, Advanced, Expert }
        public enum ProductStatus { Ok = 1, Deteriorated, Broken }
        public enum ProductType { Machine = 1, Article }
        public enum Genre { Mujer = 1, Hombre }
        public enum AgeClass { YoungAdult = 0, Adult, MiddleAge, Senior  }
        public enum Months { Enero = 1, Febrero, Marzo, Abril, Mayo, Junio, Julio, Agosto, Septiembre, Octubre, Noviembre, Diciembre }
        public enum Destinataries { Activity = 1, ClientsWithDebt, Clientes, Admins, Profesores }
        
    }
}
