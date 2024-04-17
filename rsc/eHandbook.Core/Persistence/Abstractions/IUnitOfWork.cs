using eHandbook.Core.Persistence.Repositories.Common;

namespace eHandbook.Core.Persistence.Abstractions
{
    /// <summary>
    /// It has to receive a Typeof IRepositoryName.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitOfWork<T> where T : class
    {

        public GenericBaseRepository<T> GetRepository { get; }
        // As best practrice I am using CancellationToken is used to cancel a task if it’s no longer needed or if it’s taking too long to complete.
        // This helps to prevent resource wastage and improve the overall performance of the application
        Task<bool> SaveAsync(CancellationToken cancellationToken);
        //Not implemented as it's not necessary for now. Non error
        //void Dispose();
        //void Rollback();
    }
}