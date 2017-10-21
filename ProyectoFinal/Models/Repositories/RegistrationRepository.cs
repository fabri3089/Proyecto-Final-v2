using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class RegistrationRepository:IRegistrationRepository, IDisposable
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
            return context.Registrations.Include(r => r.Client)
                                        .Include(r => r.Group)
                                        .ToList();
        }

        public Registration GetRegistrationByID(int id)
        {
            return context.Registrations.Find(id);
        }

        public void InsertRegistration(Registration registration)
        {
            context.Registrations.Add(registration);
        }

        public void DeleteRegistration(int id)
        {
            Registration registration = context.Registrations.Find(id);
            context.Registrations.Remove(registration);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateRegistration(Registration Registration)
        {
            context.Entry(Registration).State = EntityState.Modified;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        public void EliminarInscripto(int groupID)
        {
            List<Group> groups = context.Groups.ToList();
            var a = context.Groups.Where(r => r.GroupID == groupID).FirstOrDefault();
            if (a != null)
            {
                a.Amount = a.Amount - 1;
            }


        }
        public bool HorarioClase(int clientID, int groupID)
        {
            
            List<Group> groupClient = new List<Group>();
            List<Group> groups = context.Groups.ToList();
            foreach(var group in groups)
            {
                int id = group.GroupID;
                var registration = context.Registrations.Where(r => r.ClientID == clientID && r.GroupID == id).FirstOrDefault();
                if (registration != null) groupClient.Add(group);
                registration = null;
            }

            var gr = context.Groups.Where(g => g.GroupID == groupID).FirstOrDefault();
            var a = groupClient.Where(k => k.Day == gr.Day && gr.HourFrom >= k.HourFrom && gr.HourFrom < k.HourTo).FirstOrDefault();
            if (a == null)
            {
                return false;
            }
            return true;
        }

       
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
