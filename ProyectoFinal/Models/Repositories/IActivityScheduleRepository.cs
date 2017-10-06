using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IActivityScheduleRepository : IDisposable
    {
        IEnumerable<ActivitySchedule> GetActivitySchedules();
        ActivitySchedule GetActivityScheduleByID(int activityScheduleID);
        void InsertActivitySchedule(ActivitySchedule activitySchedule);
        void DeleteActivitySchedule(int activityScheduleID);
        void UpdateActivitySchedule(ActivitySchedule activitySchedule);
        void Save();
    }
}
