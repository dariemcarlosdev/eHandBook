using eHandbook.Core.Persistence.Repositories.Common;
using eHandbook.Core.Validations.Common.GuardValidation;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eHandbook.modules.ManualManagement.Infrastructure.Persistence.Repository
{
    public class ManualRepository : GenericBaseRepository<ManualEntity>/*, IManualRepository*/
    {


        #region constructor
        /// <summary>
        /// Passing to the Class GenericRepository's(Base Class) constructor the ManualDbContext as parameter.
        /// </summary>
        /// <param name="_manualDbContext"></param>
        public ManualRepository(ManualDbContext manualDbContext) : base(manualDbContext)
        {

        }

        #endregion

        #region Interface methods implementation and its owns methods.
        /// <summary>
        /// Create a new Manual.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> CreateManualAsync(ManualEntity manual)
        {
            EnsureValidation.AgainstNullorEmpty(manual, "Manual cannot be null.");

            return await CreateEntityAsync(manual);
        }


        /// <summary>
        /// Get all Manuals
        /// </summary>
        /// <returns>List Manuals ordered by Creation Date</returns>
        public async Task<IEnumerable<ManualEntity>> GetAllManualsAsync()
        {
            return await GetAllEntitiesQueryable().
                            OrderBy(manual => manual.CreatedOn).
                            ToListAsync();
        }


        /// <summary>
        /// Get a Manual by Guid.
        /// </summary>
        /// <param name="manualGuid"></param>
        /// <returns>Manual</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ManualEntity> GetManualByGuidAsync(Guid id)
        {
            EnsureValidation.AgainstNullorEmpty(id);
            return await FindEntityByQueryable(manual => manual.Id == id)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get Manual by Id.
        /// </summary>
        /// <param name="manualId"></param>
        /// <returns>Manual</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ManualEntity> GetManualByIdAsync(Guid manualId)
        {
            EnsureValidation.AgainstNullorEmpty(manualId);
            return await FindEntityByQueryable(manual => manual.Id == manualId)
                    .FirstOrDefaultAsync();

        }


        /// <summary>
        /// Hard delete Manual form DB.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> DeleteManualAsync(ManualEntity manual)
        {
            var manualExist = await GetManualByIdAsync(manual.Id);

            EnsureValidation.AgainstNullorEmpty(manualExist, "The search return null, hence a manual with given id does not exist.");

            manualExist.DeletedOn = DateTime.Now;
            manualExist.DeletedBy = "@User";
            return DeleteEntity(manualExist);
        }


        /// <summary>
        /// Hard delete Manual form DB.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> DeleteManual(Guid id)
        {

            var manualExist = await GetManualByIdAsync(id);

            EnsureValidation.AgainstNullorEmpty(manualExist, "The search return null, hence a manual with given id does not exist.");

            manualExist.DeletedOn = DateTime.Now;
            manualExist.DeletedBy = "@User";
            return DeleteEntity(manualExist);
        }


        /// <summary>
        /// Update a Manual in DB.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        //public bool UpdateManual(ManualEntity manual)
        //{
        //    EnsureValidation.AgainstNullorEmpty(manual, "Update fails. Manual cannot be null.");

        //    manual.IsUpdated = true;
        //    manual.UpdatedOn = DateTime.UtcNow;
        //    manual.UpdatedBy = "@User";
        //    return UpdateEntity(manual);

        //}


        /// <summary>
        /// Return true/false if manual exists or not.Check By manualId.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<bool> ManualExistAsync(Guid Id)
        {
            EnsureValidation.AgainstNullorEmpty(Id, "Id cannot be null, the search could not be executed.");
            return await FindEntityByQueryable(manual => manual.Id == Id).AnyAsync();

        }


        /// <summary>
        /// Return true/false if manual exists or not. Check By ManualEntity.
        /// </summary>
        /// <param name="manual"></param>
        /// <returns>bool</returns>
        public async Task<bool> ManualExistAsync(ManualEntity manual)
        {
            EnsureValidation.AgainstNullorEmpty(manual, "Manual is null, cannot check the manual existence.");
            return await DoesEntityExist(manual);

        }


        #endregion


        #region MethodNoImplemented
        //public void SoftDeleteManual(ManualEntity manual)
        //{
        //    if (manual == null)
        //    {
        //        throw new ArgumentNullException(nameof(manual));
        //    }
        //    SoftDelete(manual);
        //}


        //public Task<ManualEntity> GetManualWithCategoriesAsync(Guid manualGuid)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

    }
}
