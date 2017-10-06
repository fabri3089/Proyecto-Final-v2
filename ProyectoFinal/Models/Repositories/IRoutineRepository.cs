using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IRoutineRepository : IDisposable
    {
        IEnumerable<Routine> GetRoutines();
        Routine GetRoutineByID(int routineID);
        IEnumerable<Routine> GetRoutinesByClientID(int clientID);
        void InsertRoutine(Routine routine);
        void DeleteRoutine(int routineID);
        void UpdateRoutine(Routine routine);
        void Save();
    }
}
