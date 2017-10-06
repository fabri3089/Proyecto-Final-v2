using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class MedicalRecordRepository : IMedicalRecordRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public MedicalRecordRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<MedicalRecord> GetMedicalRecords()
        {
            return context.MedicalRecords.Include(m => m.Client).ToList();
        }

        public MedicalRecord GetMedicalRecordByID(int id)
        {
            return context.MedicalRecords.Include(m => m.Client).Where(m => m.MedicalRecordID == id).FirstOrDefault();
        }

        public void InsertMedicalRecord(MedicalRecord medicalRecord)
        {
            context.MedicalRecords.Add(medicalRecord);
        }

        public void DeleteMedicalRecord(int id)
        {
            MedicalRecord medicalRecord = context.MedicalRecords.Find(id);
            context.MedicalRecords.Remove(medicalRecord);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateMedicalRecord(MedicalRecord medicalRecord)
        {
            context.Entry(medicalRecord).State = EntityState.Modified;
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