using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Validation;

namespace eHandbook.Core.Persistence.Repositories.Common
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        //ensuring there is only one DbContext Instance per Request/Unit of Work/Transaction
        private readonly DbContext _context;
        //private bool _disposed = false;
        private string _errorMessage = string.Empty;

        /// <summary>
        /// Get current repository.
        /// </summary
        public GenericBaseRepository<T> GetRepository { get; /*private set; */}

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{T}"> class.
        /// </summary>
        /// <param name="manualDbContext">The current DbContext</param>
        //Using the Constructor we are initializing the current Context and ManualRepository Properties which are declared in the IUnitOfWork Interface
        //This is nothing but we are storing the DBContext (ManualDBContext) and ManualRepository objects in GetCurrentDbContext Property and GetRepository
        public UnitOfWork(DbContext manualDbContext)
        {
            _context = manualDbContext;
            if (GetRepository == null)
            {
                GetRepository = new GenericBaseRepository<T>(_context);
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


        //we are not extending Dispose() method from IDisposable Iterface,  since as a general rule, if the Dependency Injection container creates an instance of the disposable object(ManualDbContext),
        //it will clean up when the instance lifetime (transient, scoped or singleton) expires (E.g. for scoped instances in ASP.NET Core,
        //this will be at the end of the request/response lifetime but for singletons, it is when the container itself is disposed).

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!_disposed)
        //    {
        //        if (disposing)
        //        {
        //            _context.Dispose();
        //        }
        //    }
        //    _disposed = true;
        //}

    }

    //This Rollback method it's not necessary for now.

    //public void Rollback()
    //{
    //    throw new NotImplementedException();
    //}
}

