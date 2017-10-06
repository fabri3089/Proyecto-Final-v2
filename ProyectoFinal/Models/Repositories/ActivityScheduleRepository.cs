using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class ActivityScheduleRepository : IActivityScheduleRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public ActivityScheduleRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface Implementation
        public IEnumerable<ActivitySchedule> GetActivitySchedules()
        {
            return context.ActivitySchedules.Include(a => a.Activity).ToList();
        }

        public ActivitySchedule GetActivityScheduleByID(int id)
        {
            return context.ActivitySchedules.Include(a => a.Activity).Where(a => a.ActivityScheduleID == id).FirstOrDefault();
        }

        public void InsertActivitySchedule(ActivitySchedule activitySchedule)
        {
            context.ActivitySchedules.Add(activitySchedule);
        }

        public void DeleteActivitySchedule(int id)
        {
            ActivitySchedule activitySchedule = context.ActivitySchedules.Find(id);
            context.ActivitySchedules.Remove(activitySchedule);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateActivitySchedule(ActivitySchedule activitySchedule)
        {
            context.Entry(activitySchedule).State = EntityState.Modified;
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