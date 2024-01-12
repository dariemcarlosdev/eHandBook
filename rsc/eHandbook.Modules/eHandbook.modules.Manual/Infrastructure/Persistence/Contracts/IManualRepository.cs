using eHandbook.Core.Persistence.Repositories.Common;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;

namespace eHandbook.modules.ManualManagement.Infrastructure.Persistence.Contracts
{
    public interface IManualRepository : IGenericRepository<ManualEntity>
    {
        #region MethodImplemented
        /// <summary>
        /// Get List of Manuals
        /// </summary>
        /// <returns>List Manuals ordered by CreatedOn prop.</returns>
        Task<IEnumerable<ManualEntity>> GetAllManualsAsync();

        /// <summary>
        /// Get Manual by Id.
        /// </summary>
        /// <param name="manualId"></param>
        /// <returns>Manual</returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ManualEntity> GetManualByIdAsync(Guid manualId);


        /// <summary>
        /// Create a new Manual.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        Task<bool> CreateManualAsync(ManualEntity manual);

        /// <summary>
        /// Update a Manual in DB.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        //bool UpdateManual(ManualEntity manual);

        /// <summary>
        /// Hard delete Manual from DB.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        Task<bool> DeleteManualAsync(ManualEntity manual);

        /// <summary>
        /// Overload Method. Hard delete Manual found by Id from DB.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException"></exception>
        Task<bool> DeleteManual(Guid id);

        /// <summary>
        /// Return True/False if record exist
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<bool> ManualExistAsync(Guid Id);

        #endregion MethodImplemented



        #region NoImplementedMethod
        /// <summary>
        /// Get Manual with categories.
        /// </summary>
        /// <param name="manualGuid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //public Task<ManualEntity> GetManualWithCategoriesAsync(Guid manualGuid)
        //{

        //    //When Categories property included in Manual Entity.
        //    //return await FindByCondition( manual => manual.Id.Equals(manualGuid))
        //    //.Include(c => c.Categories)
        //    //.FirstOrDefaultAsync();

        //    throw new NotImplementedException();
        //}

        /// <summary>
        /// Mark the Manual propery IsDeleted as true.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="NotImplementedException"></exception>
        //void SoftDeleteManual(ManualEntity manual);        


        /// <summary>
        /// Get Manual with categories as details.
        /// </summary>
        /// <param name="manualGuid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //Task<ManualEntity> GetManualWithCategoriesAsync(Guid manualGuid);
        #endregion
    }
}
