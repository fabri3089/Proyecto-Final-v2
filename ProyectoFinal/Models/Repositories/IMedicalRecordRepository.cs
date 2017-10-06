using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IMedicalRecordRepository : IDisposable
    {
        IEnumerable<MedicalRecord> GetMedicalRecords();
        MedicalRecord GetMedicalRecordByID(int medicalRecordID);
        void InsertMedicalRecord(MedicalRecord medicalRecord);
        void DeleteMedicalRecord(int medicalRecordID);
        void UpdateMedicalRecord(MedicalRecord medicalRecord);
        void Save();
    }
}
