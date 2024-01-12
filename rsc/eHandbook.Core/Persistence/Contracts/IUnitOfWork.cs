namespace eHandbook.Core.Persistence.Contracts
{
    /// <summary>
    /// It has to receive a Typeof IRepositoryName.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUnitOfWork<T> : IDisposable where T : class
    {

        public GenericBaseRepository<T> GetRepository { get; }

        new void Dispose();
        Task<bool> SaveAsync();
        //Not implemented as it's not necessary for now.
        //void Rollback();
    }
}