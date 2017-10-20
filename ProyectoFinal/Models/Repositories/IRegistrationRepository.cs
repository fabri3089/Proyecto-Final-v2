using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
   public interface IRegistrationRepository:IDisposable
    {
        IEnumerable<Registration> GetRegistrations();
        Registration GetRegistrationByID(int registrationID);
        void InsertRegistration(Registration registration);
        void DeleteRegistration(int registrationID);
        void UpdateRegistration(Registration registration);
        void EliminarInscripto(int groupID);
        void Save();
    }
}
