using eHandbook.Core.Persistence.Contracts;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.Infrastructure.Persistence.Repository
{
    public interface IManualRepository : IGenericRepository<ManualEntity>
    {
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
        Task<ManualEntity> GetManualByIdAsync(int manualId);

        /// <summary>
        /// Get a Manual by Guid.
        /// </summary>
        /// <param name="manualGuid"></param>
        /// <returns>Manual</returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<ManualEntity> GetManualByGuidAsync(Guid manualGuid);

        /// <summary>
        /// Get Manual with categories.
        /// </summary>
        /// <param name="manualGuid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        //Implement this method when new Category Model would be include in Domain.
        Task<ManualEntity> GetManualWithDetailsAsync(Guid manualGuid);

        /// <summary>
        /// Create a new Manual.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        void CreateManual(ManualEntity manual);

        /// <summary>
        /// Update a Manual in DB.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        void UpdateManual(ManualEntity manual);

        /// <summary>
        /// Hard delete Manual form DB.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="ArgumentNullException"></exception>
        void HardDeleteManual(ManualEntity manual);

        /// <summary>
        /// Mark the Manual propery IsDeleted as true.
        /// </summary>
        /// <param name="manual"></param>
        /// <exception cref="NotImplementedException"></exception>
        void SoftDeleteManual(ManualEntity manual);
    }
}
