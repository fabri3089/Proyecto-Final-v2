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
        Registration FindRegistration(int? id);
        void InsertRegistration(Registration registration);
        void DeleteRegistration(int registrationID);
        void UpdateRegistration(Registration registration);
        bool ValidarCupo(int groupID);
        bool ValidarNivel(int groupID);
        bool ValidarAbonoActivo(int clientID, int activityID);
        bool HorarioClase(int clientID, int groupID);
        void Save();
    }
}
