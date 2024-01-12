using System.Linq.Expressions;

namespace eHandbook.Core.Persistence.Contracts
{
    /// <summary>
    /// Interface that defines the contract for data access operations.
    /// This interface should include methods for common operations like GetById, GetAll, Add, Update, and Delete. 
    /// </summary>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Add new Entity to DB and save changes.
        /// </summary>
        /// <param name="entityToInsert"></param>
        Task<bool> CreateEntity(TEntity entityToInsert);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>return IQueryable that allows us to attach async calls to them.</returns>
        IQueryable<TEntity> GetAllQueryable();

        /// <summary>
        ///  Find a Entity in the Repository by defined conditions using a lamba expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>return IQueryable that allows us to attach async calls to them.</returns>
        IQueryable<TEntity> GetEntityByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Delete Entity from Database and save changes.
        /// </summary>
        /// <param name="entityToDelete"></param>
        bool DeleteEntity(TEntity entityToDelete);
        /// <summary>
        /// Update a Entity in the Repository and save changes.
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        bool UpdateEntity(TEntity entityToUpdate);

        /// <summary>
        /// Check if Entity Exist in Repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DoesEntityExist(TEntity entity);

        ///// <summary>
        ///// Updade delete status to true.
        ///// </summary>
        ///// <param name="entity"></param>
        //void SoftDelete(T entity);
    }
}
