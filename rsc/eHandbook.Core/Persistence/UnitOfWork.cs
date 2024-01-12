using eHandbook.Core.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;

namespace eHandbook.Core.Persistence
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        //ensuring there is only one DbContext Instance per Request/Unit of Work/Transaction
        private readonly DbContext _context;
        private bool _disposed = false;
        private string _errorMessage = string.Empty;

        /// <summary>
        /// Get current repository.
        /// </summary
        public GenericBaseRepository<T> GetRepository { get; /*private set; */}


        //Using the Constructor we are initializing the current Context and ManualRepository Properties which are declared in the IUnitOfWork Interface
        //This is nothing but we are storing the DBContext (ManualDBContext) and ManualRepository objects in GetCurrentDbContext Property and GetRepository
        public UnitOfWork(DbContext manualDbContext)
        {
            _context = manualDbContext;
            if (this.GetRepository == null)
            {
                this.GetRepository = new GenericBaseRepository<T>(_context);
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                var isSaved = await _context!.SaveChangesAsync();
                return isSaved >= 0;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
                    }
                }
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

    }

    //This Rollback method it's not necessary for now.
    
    //public void Rollback()
    //{
    //    throw new NotImplementedException();
    //}
}

