using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IAssistanceRepository : IDisposable
    {
        IEnumerable<Assistance> GetAssistances();
        Assistance GetAssistanceByID(int assistanceID);
        void InsertAssistance(Assistance assistance);
        void DeleteAssistance(int assistanceID);
        void UpdateAssistance(Assistance assistance);
        void Save();
    }
}
