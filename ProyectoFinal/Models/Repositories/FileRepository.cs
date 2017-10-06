using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProyectoFinal.Models.Repositories
{
    public class FileRepository : IFileRepository, IDisposable
    {
        #region Properties
        public GymContext context;
        private bool disposed = false;
        #endregion

        #region Constructors
        public FileRepository(GymContext context)
        {
            this.context = context;
        }
        #endregion

        #region Interface implementation
        public IEnumerable<File> GetFiles()
        {
            return context.Files.Include(f => f.Routine).ToList();
        }

        public File GetFileByID(int id)
        {
            return context.Files.Include(f => f.Routine).Where(f => f.FileID == id).FirstOrDefault();
        }

        public void InsertFile(File file)
        {
            context.Files.Add(file);
        }

        public void InsertListOfFiles(List<File> files)
        {
            context.Files.AddRange(files);
        }

        public void DeleteFile(int id)
        {
            File file = context.Files.Find(id);
            context.Files.Remove(file);
        }

        public void DeleteFilesByRoutineID(int routineID)
        {
            List<File> files = context.Files.Where(f => f.RoutineID == routineID).ToList();
            foreach (var file in files)
            {
                context.Files.Remove(file);
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateFile(File file)
        {
            context.Entry(file).State = EntityState.Modified;
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