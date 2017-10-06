using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class AssistanceRepository : IAssistanceRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public AssistanceRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface Implementation
        public IEnumerable<Assistance> GetAssistances()
        {
            return context.Assistances.ToList();
        }

        public Assistance GetAssistanceByID(int id)
        {
            return context.Assistances.Find(id);
        }

        public void InsertAssistance(Assistance assistance)
        {
            context.Assistances.Add(assistance);
        }

        public void DeleteAssistance(int id)
        {
            Assistance assistance = context.Assistances.Find(id);
            context.Assistances.Remove(assistance);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateAssistance(Assistance assistance)
        {
            context.Entry(assistance).State = EntityState.Modified;
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