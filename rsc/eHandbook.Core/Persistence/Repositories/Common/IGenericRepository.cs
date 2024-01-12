using System.Linq.Expressions;

namespace eHandbook.Core.Persistence.Repositories.Common
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
        Task<bool> CreateEntityAsync(TEntity entityToInsert);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns>return IQueryable that allows us to attach async calls to them.</returns>
        IQueryable<TEntity> GetAllEntitiesQueryable();

        /// <summary>
        ///  Find a Entity in the Repository by defined conditions using a blueprint lamba expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>return IQueryable that allows us to attach async calls to them.</returns>
        IQueryable<TEntity> FindEntityByQueryable(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Delete Entity from Database and save changes.
        /// </summary>
        /// <param name="entityToDelete"></param>
        bool DeleteEntity(TEntity entityToDelete);

        /// <summary>
        /// Delete data by using blueprint lambda expresion.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        void DeleteEntityByCondition(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// Update a Entity in the Repository.
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        bool UpdateEntity(TEntity? entityToUpdate/*, object entityToUpdateRequest*/);

        /// <summary>
        /// Check if Entity Exist in Repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns name="bool"></returns>
        Task<bool> DoesEntityExist(TEntity entity);

        /// <summary>
        /// Count all Entties.
        /// </summary>
        /// <returns></returns>
        int CountEntities();

        /// <summary>
        /// Find all Entities from Repository.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>IQueryable</returns>
        Task<ICollection<TEntity>> GetAllEntitiesByConditionAsync(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Get all Entities Async.
        /// </summary>
        /// <returns></returns>
        Task<ICollection<TEntity>> GetAllEntitiesAsync();

        /// <summary>
        /// Count all Entities async.
        /// </summary>
        /// <returns></returns>
        Task<int> CountEntitiesAsync();

        /// <summary>  
        /// Gets a single record using lambda expression. (usually the unique identifier)  
        /// </summary>  
        /// <param name="expression">Criteria to match on</param>  
        /// <returns>A single record that matches the specified lamda expression</returns>  
        Task<TEntity?> FindEntityAsync(Expression<Func<TEntity, bool>> expression);


        #region GMleakOfImplement
        //TEntity FindEntity(Expression<Func<TEntity, bool>> filter, IList<Expression<Func<TEntity, object>>> includedProperties = null);
        //PaginatedList<T> GetPaged(Expression<Func<T, bool>> filter = null, IList<Expression<Func<T, object>>> includedProperties = null, PagingOptions<T> pagingOptions = null);
        //IEnumerable<T> Get(Expression<Func<T, bool>> filter = null, IList<Expression<Func<T, object>>> includedProperties = null, IList<ISortCriteria<T>> sortCriterias = null);
        //void DeleteEntity(Expression<Func<TEntity, bool>> filter);
        #endregion

    }
}
