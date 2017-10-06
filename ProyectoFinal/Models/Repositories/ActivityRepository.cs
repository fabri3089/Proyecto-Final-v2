using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class ActivityRepository : IActivityRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public ActivityRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<Activity> GetActivities()
        {
            return context.Activities
                                     .Include(a => a.ActivitySchedules)
                                     .Include(a => a.PaymentTypes)
                                     .ToList();
        }

        public Activity GetActivityByID(int id)
        {
            return context.Activities.Where(a => a.ActivityID == id)
                                     .Include(a => a.ActivitySchedules)
                                     .Include(a => a.PaymentTypes)
                                     .FirstOrDefault();
        }

        public void InsertActivity(Activity activity)
        {
            context.Activities.Add(activity);
        }

        public void DeleteActivity(int id)
        {
            Activity activity = context.Activities.Find(id);
            context.Activities.Remove(activity);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateActivity(Activity activity)
        {
            context.Entry(activity).State = EntityState.Modified;
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