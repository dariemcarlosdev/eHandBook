using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;

namespace eHandbook.modules.ManualManagement.Application.Contracts
{
    public interface IManualService
    {
        #region Interface method definition.

        /// <summary>
        /// Service to Add new Manual Entity to DataBase.
        /// </summary>
        /// <param name="manual"></param>
        /// <returns>ManualDto</returns>       
        Task<ResponderService<ManualDto>> AddNewManualAsync(ManualToCreateDto manualCreateDtoRequest);

        /// <summary>
        /// Async Service Get Manual found by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        Task<ResponderService<ManualDto>> GetManualByIdAsync(Guid id);

        /// <summary>
        /// Async Service to Get All Manual Entities which are not marked as deleted.
        /// </summary>
        /// <returns>List of ManualDto</returns>
        Task<ResponderService<IEnumerable<ManualDto>>> GetAllManualsAsync();

        /// <summary>
        /// Service for Update a Manual Entity in DataBase. Set entity IsUpdated to True.
        /// </summary>
        /// <param name="manualToUpdateDtoRequest"></param>
        /// <returns>ManualDto</returns>
        Task<ResponderService<ManualDto>> UpdateManualAsyn(ManualToUpdateDto manualToUpdateDtoRequest);

        /// <summary>
        /// Service for delete a Manual Entity from DataBase.
        /// </summary>
        /// <param name="manualToDeleteDtoRequest"></param>
        /// <returns>string</returns>
        Task<ResponderService<string>> DeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest);

        /// <summary>
        /// DeleteManual() method overloading. This Service delete a Manual Entity found by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        Task<ResponderService<ManualDto>> DeleteManualAsync(Guid id);

        /// <summary>
        /// Service to change prop. IsDeleted to tru in Manual Entity, no hard delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        Task<ResponderService<ManualDto>> SoftDeleteManualAsync(Guid id);

        Task<ResponderService<ManualDto>> SoftDeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest);

        #endregion
    }
}