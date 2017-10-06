using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Models.Repositories
{
    public interface IFileRepository : IDisposable
    {
        IEnumerable<File> GetFiles();
        File GetFileByID(int fileID);
        void InsertFile(File file);
        void InsertListOfFiles(List<File> files);
        void DeleteFile(int fileID);
        void DeleteFilesByRoutineID(int routineID);
        void UpdateFile(File file);
        void Save();
    }
}
