﻿using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;

namespace eHandbook.modules.ManualManagement.Application.Abstractions
{
    public interface IManualService
    {
        #region Interface method definition.

        /// <summary>
        /// Service to Add new Manual Entity to DataBase.
        /// </summary>
        /// <param name="manual"></param>
        /// <returns>ManualDto</returns>       
        Task<ApiResponseService<ManualDto>> AddNewManualAsync(ManualToCreateDto manualCreateDtoRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Async Service Get Manual found by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        Task<ApiResponseService<ManualDto>> GetManualByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Async Service to Get All Manual Entities which are not marked as deleted.
        /// </summary>
        /// <returns>List of ManualDto</returns>
        Task<ApiResponseService<IEnumerable<ManualDto>>> GetAllManualsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Service for Update a Manual Entity in DataBase. Set entity IsUpdated to True.
        /// </summary>
        /// <param name="manualToUpdateDtoRequest"></param>
        /// <returns>ManualDto</returns>
        Task<ApiResponseService<ManualDto>> UpdateManualAsyn(ManualToUpdateDto manualToUpdateDtoRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Service for delete a Manual Entity from DataBase.
        /// </summary>
        /// <param name="manualToDeleteDtoRequest"></param>
        /// <returns>string</returns>
        Task<ApiResponseService<string>> DeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest, CancellationToken cancellationToken);

        /// <summary>
        /// DeleteManual() method overloading. This Service delete a Manual Entity found by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        Task<ApiResponseService<ManualDto>> DeleteManualByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Service to change prop. IsDeleted to tru in Manual Entity, no hard delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        Task<ApiResponseService<ManualDto>> SoftDeleteManualByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<ApiResponseService<ManualDto>> SoftDeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest, CancellationToken cancellationToken);

        #endregion
    }
}