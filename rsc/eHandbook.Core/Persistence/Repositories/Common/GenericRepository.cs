using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eHandbook.Core.Persistence.Repositories.Common
{

    /// <summary>
    /// Base or Generic Repository implements the Generic Repository interface. 
    /// These classes interact with the underlying data storage and provide the necessary operations for data retrieval and manipulation.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericBaseRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class

    {
        #region property
        internal readonly DbContext _dbContext;
        internal readonly DbSet<TEntity> _dbSet;

        #endregion

        #region ctr
        /// <summary>
        /// If you don't want to use Unit of Work, then use the following Constructor 
        /// which takes the context Object as a parameter
        /// </summary>
        /// <param name="dbContext"></param>
        /// <exception cref="ArgumentNullException"></exception>

        public GenericBaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        #endregion

        //Refactored.
        /// <summary>
        /// Generic Add new Entity to the Reposity.The changes will be performed through out UoW Pattern implementation.
        /// </summary>
        /// <param name="entityToInsert"></param>
        /// <returns>bool</returns>
        public async Task<bool> CreateEntityAsync(TEntity entityToInsert)
        {
            try
            {

                if (entityToInsert == null)
                {
                    throw new ArgumentNullException(nameof(entityToInsert));
                }



                return await _dbSet.AddAsync(entityToInsert) is not null;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Generic Hard Delete method Entity in the Repository.The changes will be performed through out UoW Pattern implementation.
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <returns>bool</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool DeleteEntity(TEntity? entityToDelete)
        {
            if (entityToDelete == null)
            {
                throw new ArgumentNullException(nameof(entityToDelete));
            }
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            return true;
        }

        //Refactored.
        /// <summary>  
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// The changes will be performed through out UoW Pattern implementation.
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns>void</returns>  
        public void DeleteEntityByCondition(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> objects = _dbSet.Where(where).AsQueryable();
            foreach (TEntity obj in objects)
                _dbSet.Remove(obj);
        }

        //Refactored.
        /// <summary>  
        /// generic Update method , updates data for the entities on the basis of condition.
        /// The changes will be performed through out UoW Pattern implementation.
        /// </summary>  
        /// <param name="where"></param>  
        /// <returns></returns>  
        public void UpdateByCondition(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> objects = _dbSet.Where(where).AsQueryable();
            foreach (TEntity obj in objects)
                _dbSet.Update(obj);
        }

        //Refactored.
        /// <summary>
        /// Update a Entity in the Repository.The changes will be performed through out UoW Pattern implementation.
        /// </summary>
        /// <param name="entityToUpdate"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>bool</returns>
        public bool UpdateEntity(TEntity? entityToUpdate)
        {
            try
            {
                if (entityToUpdate == null)
                {
                    return false;
                    throw new ArgumentNullException(nameof(entityToUpdate));
                }

                _dbSet.Update(entityToUpdate);
                _dbSet.Entry(entityToUpdate).State = EntityState.Modified;
                return true;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

        }

        //Refactored
        /// <summary>
        /// Find a Entity in the Repository by defined conditions using a lamba expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>IQueryable</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IQueryable<TEntity>? FindEntityByQueryable(Expression<Func<TEntity, bool>>? expression)
        {
            try
            {
                IQueryable<TEntity> queryResult = _dbSet;

                if (expression != null)
                {

                    queryResult = queryResult.AsQueryable()
                   .Where(expression);

                    return queryResult.AsNoTracking();
                }

                return null;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        //Refactored.
        /// <summary>  
        /// Inclue multiple  
        /// </summary>  
        /// <param name="predicate"></param>  
        /// <param name="include"></param>  
        /// <returns></returns>  
        public IQueryable<TEntity> GetAllWithIncludeQueryble(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate).AsNoTracking();
        }

        //Refactored.
        /// <summary>
        /// .Overloaded Method.Another way of GetAllWithInclude implementation
        /// </summary>
        /// <param name="includeProperties"></param>
        /// <returns>IQueryable<TEntity></returns>
        public IQueryable<TEntity> GetAllWithIncludeQueryble(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = (IQueryable<TEntity>)GetAllEntitiesAsync();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }
            return queryable;
        }

        //Refactored.
        /// <summary>
        /// Find all Entities from Repository.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns>IQueryable</returns>
        public async Task<ICollection<TEntity>> GetAllEntitiesByConditionAsync(Expression<Func<TEntity, bool>> expression)
        {
            // This optimisation allows you to tell Entity Framework not to track the results of a query. This means that Entity Framework performs no additional processing or storage of the entities which are returned by the query.
            // However, it also means that you can't update these entities without reattaching them to the tracking graph.
            // The AsNoTracking() extension method returns a new query and the returned entities will not be cached by the context (DbContext or Object Context). This means that the Entity Framework does not perform any additional processing or storage of the entities that are returned by the query.
            // Please note that we cannot update these entities without attaching to the context. Mostly used with READ-ONLY queries.
            // eg. Sometimes we do not want to track some entities because the data is only used for viewing purposes and other operations such as insert, update, and delete are not done.
            // For example the view data in a read-only grid.
            return await _dbSet.Where(expression).AsNoTracking().ToListAsync();
        }

        //Refactored.
        /// <summary>
        /// Get all Entities Async.
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<TEntity>> GetAllEntitiesAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        //Refactored
        /// <summary>  
        /// Get Entities based on predicate - lamda Empression.  
        /// </summary>  
        /// <param name="expression"></param>  
        /// <returns></returns>  
        public IQueryable<TEntity> GetAllEntitiesQueryable(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression).AsQueryable().AsNoTracking();
        }

        //Refactored.
        /// <summary>
        /// Get all Entities as Queryble.
        /// </summary>
        /// <returns>IQueryable</returns>
        public IQueryable<TEntity> GetAllEntitiesQueryable()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }

        //Refactored.
        /// <summary>  
        /// Gets a single record using lambda expression. (usually the unique identifier)  
        /// </summary>  
        /// <param name="expression">Criteria to match on</param>  
        /// <returns>A single record that matches the specified lamda expression</returns>  
        public async Task<TEntity?> FindEntityAsync(Expression<Func<TEntity, bool>> expression)
        {
            // Return first element of a collection that satisfy a specified lambda expression.
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(expression);

        }

        /// <summary>
        /// Check if Entity Exist in Repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>bool</returns>
        public async Task<bool> DoesEntityExist(TEntity entity)
        {

            var result = await _dbSet.AsNoTracking().AnyAsync(e => e == entity);
            return result;

        }

        //Refactored
        public int CountEntities()
        {
            return _dbSet.Count();
        }

        //Refactored
        public async Task<int> CountEntitiesAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
