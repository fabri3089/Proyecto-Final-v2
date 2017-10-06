using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class RoutineRepository : IRoutineRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public RoutineRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<Routine> GetRoutines()
        {
            return context.Routines.Include(r => r.Client).ToList();
        }

        public Routine GetRoutineByID(int id)
        {
            return context.Routines.Include(r => r.Client).Where(r => r.RoutineID == id).FirstOrDefault();
        }

        public IEnumerable<Routine> GetRoutinesByClientID(int clientID)
        {
            return context.Routines.Include(r => r.Client).Where(r => r.ClientID == clientID).ToList();
        }

        public void InsertRoutine(Routine routine)
        {
            context.Routines.Add(routine);
        }

        public void DeleteRoutine(int id)
        {
            Routine routine = context.Routines.Find(id);
            context.Routines.Remove(routine);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateRoutine(Routine routine)
        {
            context.Entry(routine).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}