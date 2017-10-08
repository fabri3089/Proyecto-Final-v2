using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class RegistrationRepository
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public RegistrationRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<Registration> GetRegistrations()
        {
            return context.Registrations.ToList();
        }

        public Group GetGroupByID(int id)
        {
            return context.Groups.Find(id);
        }

        public void InsertGroup(Group group)
        {
            context.Groups.Add(group);
        }

        public void DeleteGroup(int id)
        {
            Group group = context.Groups.Find(id);
            context.Groups.Remove(group);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateGroup(Group Group)
        {
            context.Entry(Group).State = EntityState.Modified;
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
}