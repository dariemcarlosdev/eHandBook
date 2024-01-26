namespace eHandbook.Infrastructure.CrossCutting.Persistence.Contracts
{
    /// <summary>
    /// Interface that defines the contract for data access operations.
    /// This interface should include methods for common operations like GetById, GetAll, Add, Update, and Delete. 
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
