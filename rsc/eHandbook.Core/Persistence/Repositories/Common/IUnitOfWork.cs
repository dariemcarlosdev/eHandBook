namespace eHandbook.Core.Persistence.Repositories.Common
{
    /// <summary>
    /// It has to receive a Typeof IRepositoryName.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitOfWork<T> where T : class
    {

        public GenericBaseRepository<T> GetRepository { get; }
        Task<bool> SaveAsync();
        Task SaveAsync(CancellationToken cancellationToken);
        //Not implemented as it's not necessary for now. Non error
        //void Dispose();
        //void Rollback();
    }
}