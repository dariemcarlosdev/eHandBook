using AutoMapper;
using eHandbook.Core.Persistence.Abstractions;
using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using eHandbook.modules.ManualManagement.Infrastructure.Abstractions.Persistence;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace eHandbook.modules.ManualManagement.Application.Services
{
    public class ManualServices : IManualService
    {
        #region privateprops

        //Use it if you dont want to use Unit of Work Pattern.
        private readonly IManualRepository _manualRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// This field is going to hold a reference to IIUnitOfWork of type IManualRepository.
        /// </summary>
        //Use it if you want to use Unit of Work Pattern.
        private readonly IUnitOfWork<ManualEntity> _unitOfWork; //Using UnitOfWork

        #endregion

        #region ctor
        public ManualServices(IUnitOfWork<ManualEntity> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        //Business logic methods that utilize the repository. Keep in Mind works with DTOs as a way to encapsulate data that needs to be transferred across various layers or components in our application.

        #region Methods Implementation using UoW with Generic Repository.

        //Status: Implemented.Completed and Tested.
        public async Task<ApiResponseService<ManualDto>> AddNewManualAsync(ManualToCreateDto manualCreateDtoRequest, CancellationToken cancellationToken)

        {
            ApiResponseService<ManualDto> _response = new();

            var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(e => e.Description == manualCreateDtoRequest.Description)!.FirstOrDefaultAsync(cancellationToken);

            try
            {

                //Check if Manual record exist.
                if (_existingManual != null)
                {
                    _response.MetaData.Message = "This Manual already Exist, hence cannot be created.";
                    _response.MetaData.Succeeded = false;
                    _response.Data = null;
                    return _response;
                }

                var _newManual = new ManualEntity()
                {


                    Description = manualCreateDtoRequest.Description,
                    Path = manualCreateDtoRequest.Path

                };

                //Add new Manual Record
                if (!await _unitOfWork.GetRepository.CreateEntityAsync(_newManual))
                {
                    _response.MetaData.Message = "Repository Error. A new manual could not be created.";
                    _response.MetaData.Succeeded = false;
                    _response.Data = null;
                    return _response;
                }

                else
                {
                    await _unitOfWork.SaveAsync(cancellationToken);
                    _response.MetaData.Succeeded = true;
                    _response.Data = _mapper.Map<ManualDto>(_newManual);
                    _response.MetaData.Message = "Reponse OK!. Manual Created successfuly.";

                }

            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error Response.";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };

            }

            return _response;

        }

        //Status:implemented.Completed and Tested.
        /// <summary>
        /// Get Manual by Id Service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> GetManualByIdAsync(Guid id, CancellationToken cancellationToken)
        {

            ApiResponseService<ManualDto> _response = new();

            try
            {
                //check if manual record exists finding by condition using Id.
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(c => c.Id.Equals(id))!.FirstOrDefaultAsync(cancellationToken);

                if (_existingManual == null)
                {

                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manual Not Found.";
                    _response.Data = null;
                    return _response;
                }

                _response.MetaData.Succeeded = true;
                _response.MetaData.Message = "Manual Found by ID.Reponse OK";
                _response.Data = _mapper.Map<ManualDto>(_existingManual);


            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        /// <summary>
        /// Get ManualByGuid Service.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>ManualEntity</returns>
        //        public async Task<ServiceResponse<ManualEntity>> GetManualByGuidAsync(Guid guid)
        //        {
        //            ServiceResponse<ManualEntity> _response = new();

        //            try
        //            {
        //#pragma warning disable CS8603 // Possible null reference return.
        //                var manual = await _unitOfWork.GetRepository.GetEntityByCondition(c => c.Guid.Equals(guid)).FirstOrDefaultAsync();

        //                if (manual == null)
        //                {

        //                     _response.MetaData.Succeeded = false;
        //                    _response.MetaData.Message = "Manual Not Found";
        //                    return _response;
        //                }

        //                 _response.MetaData.Succeeded = true;
        //                _response.MetaData.Message = "Get Manual by Id Reponse OK";
        //                _response.Data = manual;
        //#pragma warning restore CS8603 // Possible null reference return.

        //            }
        //            catch (Exception ex)
        //            {
        //                 _response.MetaData.Succeeded = false;
        //                _response.Data = null;
        //                _response.MetaData.Message = "Error Response getting manual by Id";
        //                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
        //            }


        //            return _response;
        //        }


        //implemented.It's already Tested.

        //Stat7s



        //Status: Implemented.
        /// <summary>
        /// Get All Manuals Service method implementation.
        /// </summary>
        /// <param name=""></param>
        /// <returns>IEnumerableManualDto</returns>
        public async Task<ApiResponseService<IEnumerable<ManualDto>>> GetAllManualsAsync(CancellationToken cancellationToken)
        {
            ApiResponseService<IEnumerable<ManualDto>> _response = new() { MetaData = new()};
            try
            {
                var manuals = await _unitOfWork.GetRepository.GetAllEntitiesAsync(cancellationToken);

                if (manuals == null)
                {

                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manuals cannot be fetched.";
                    return _response;
                }

                _response.MetaData.Succeeded = true;
                _response.MetaData.Message = "Manuals fetched OK.";
                _response.Data = _mapper.Map<ICollection<ManualDto>>(manuals);
            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error Response fetching all manuals.";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }


            return _response;
        }

        //Status: Implemented.Completed and Tested.
        /// <summary>
        /// Update Manual in DataBase Service and persist the change.
        /// </summary>
        /// <param name="manualToUpdateDtoRequest"></param>
        /// <returns>ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> UpdateManualAsyn(ManualToUpdateDto manualToUpdateDtoRequest, CancellationToken cancellationToken)
        {
            //Create a empty response object.
            var _response = new ApiResponseService<ManualDto>();

            try
            {
                //Find the manual to be updated  and Check if Manual record exists.
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(m => m.Id.Equals(manualToUpdateDtoRequest.Id))!.FirstOrDefaultAsync(cancellationToken);

                if (_existingManual == null)
                {

                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manual do not exist, hence cannot be updated.";
                    _response.Data = null;
                    _response.MetaData.MyCustomErrorMessages = new List<string>()
                    {
                    "The server cannot find the requested resource.Resource missing."
                    };
                    return _response;

                }


                _existingManual.Description = manualToUpdateDtoRequest.Description;
                _existingManual.Path = manualToUpdateDtoRequest.Path;

                if (!_unitOfWork.GetRepository.UpdateEntity(_existingManual/*, manualToUpdateDtoRequest)*/))
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Repository Error updating Manual";
                    _response.Data = null;
                    return _response;
                }

                //Update ManualRequest
                await _unitOfWork.SaveAsync(cancellationToken);
                //When using Async/Await in library methods, it’s important to use ConfigureAwait(false) to avoid deadlocks. In this case it's not needed.
                //.ConfigureAwait(false);

                // if Manual was udpated. UpdateManual return true.
                _response.MetaData.Succeeded = true;
                _response.Data = _mapper.Map<ManualDto>(_existingManual);
                _response.MetaData.Message = "Manual Updated Ok";
            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error updating Manual.";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        //Status: Implemented. Completed and  Tested.
        /// <summary>
        /// Delete a Manual from database Service and persist the change.
        /// </summary>
        /// <param name="manualToDeleteDtoRequest"></param>
        /// <returns>string</returns>
        public async Task<ApiResponseService<string>> DeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest, CancellationToken cancellationToken)
        {
            ApiResponseService<string> _response = new();

            //mapping ManualDto to Delete to ManualEntity class to delete.
            //ManualEntity _deleteManual = _mapper.Map<ManualEntity>(manualToDeleteDtoRequest);

            //Get manual Entity from repository.
            ManualEntity? _deleteManual = await _unitOfWork.GetRepository.FindEntityAsync(e => e.Id == manualToDeleteDtoRequest.Id, cancellationToken);

            try
            {
                //check if Manual exist.

                if (_deleteManual == null)
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manual not exist, hence it cannot be deleted.";
                    _response.MetaData.MyCustomErrorMessages = new List<string>()
                    {
                    "The server cannot find the requested resource.Resource missing."
                    };
                    return _response;
                }

                if (!_unitOfWork.GetRepository.DeleteEntity(_deleteManual))
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Repository Error deleting Manual";
                    _response.Data = null;
                    return _response;
                }

                // if Manual was deteled. DeleteManual return true.
                await _unitOfWork.SaveAsync(cancellationToken);
                _response.MetaData.Succeeded = true;
                _response.MetaData.Message = "Respose Ok. Manual Deleted successfully.";
                _response.MetaData.MyCustomErrorMessages = new List<string>()
                {
                    "The server has successfully fulfilled the request and that there is no additional content to send in the response payload body."
                };

            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error deleting Manual.";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;

        }

        //Status: Implemented. Completed and  Tested.
        /// <summary>
        /// Method Defined in ManualService Class.Delete Manual finding by Id from database and persist the change.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> DeleteManualByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new();

            try
            {
                //check if Manual exist.
                var _manualexist = await _unitOfWork.GetRepository.FindEntityByQueryable(manual => manual.Id == id)!.SingleOrDefaultAsync(cancellationToken);

                if (_manualexist == null)
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manual not exist, hence it cannot be deleted.";
                    _response.MetaData.MyCustomErrorMessages = new List<string>()
                    {
                    "The server has successfully fulfilled the request and that there is no additional content to send in the response payload body."
                    };
                    return _response;
                }

                if (!_unitOfWork.GetRepository.DeleteEntity(_manualexist))
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Repository Error deleting Manual";
                    _response.Data = null;
                    return _response;
                }

                // if Manual was deteled. DeleteManual return true.
                await _unitOfWork.SaveAsync(cancellationToken);
                _response.Data = _mapper.Map<ManualDto>(_manualexist);
                _response.MetaData.Succeeded = true;
                _response.MetaData.Message = "Response OK. Manual was Deleted.";
            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error deleting Manual.";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        ////Status: Implement. Completed and  Tested.
        /// <summary>
        /// Find a Manual and update Manual prop. Mark Entity prop. IsDeleted to tru, no hard delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> SoftDeleteManualByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new();

            try
            {
                //check if record exist
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(manual => manual.Id == id)!.FirstOrDefaultAsync();

                if (_existingManual == null)
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manual not exist, hence it cannot be deleted.";
                    _response.Data = null;
                    _response.MetaData.MyCustomErrorMessages = new List<string>()
                    {
                    "The server has successfully fulfilled the request and that there is no additional content to send in the response payload body."
                    };
                    return _response;
                }

                //var manual = await _unitOfWork.GetRepository.FindEntityAsync(c => c.Id == id);

                ////Update prop. IsDeleted to True.
                _existingManual!.IsDeleted = true;

                if (!_unitOfWork.GetRepository.UpdateEntity(_existingManual))
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Repository Error soft deleting Manual.";
                    return _response;
                }

                await _unitOfWork.SaveAsync(cancellationToken);
                _response.MetaData.Succeeded = true;
                _response.MetaData.Message = "Response Ok. Manual Deleted successfully.";
                _response.Data = _mapper.Map<ManualDto>(_existingManual);


            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        ////Status: Implement. Completed and  Tested.
        /// <summary>
        /// Find a giving Manual and if it's found, mark prop. IsDeleted to true.
        /// </summary>
        /// <param name="manualToDeleteDtoRequest"></param>
        /// <returns></returns>
        public async Task<ApiResponseService<ManualDto>> SoftDeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new();
            try
            {
                //check if record exist
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(manual => manual.Id == manualToDeleteDtoRequest.Id)!.FirstOrDefaultAsync();

                if (_existingManual == null)
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manual not exist.";
                    _response.Data = null;
                    return _response;
                }

                ////Update prop. IsDeleted to True.
                _existingManual!.IsDeleted = true;

                if (!_unitOfWork.GetRepository.UpdateEntity(_existingManual))
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Repository Error soft deleting Manual.";
                    return _response;
                }

                await _unitOfWork.SaveAsync(cancellationToken);
                _response.MetaData.Succeeded = true;
                _response.MetaData.Message = "Response Ok. Manual Deleted successfully.";
                _response.Data = _mapper.Map<ManualDto>(_existingManual);


            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }


        public async Task<ApiResponseService<ManualDto>> SoftDeleteManualByIdAsync(Guid id, JsonPatchDocument<ManualEntity> document, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new();

            try
            {
                //check if record exist
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(manual => manual.Id == id)!.FirstOrDefaultAsync(cancellationToken);

                if (_existingManual == null)
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Manual not exist, hence it cannot be deleted.";
                    _response.Data = null;
                    return _response;
                }

                //var manual = await _unitOfWork.GetRepository.FindEntityAsync(c => c.Id == id);

                ////Update prop. IsDeleted to True.
                // _existingManual!.IsDeleted = true;

                document.ApplyTo(_existingManual);
                if (!_unitOfWork.GetRepository.UpdateEntity(_existingManual))
                {
                    _response.MetaData.Succeeded = false;
                    _response.MetaData.Message = "Repository Error soft deleting Manual.";
                    return _response;
                }

                await _unitOfWork.SaveAsync(cancellationToken);
                _response.MetaData.Succeeded = true;
                _response.MetaData.Message = "Response Ok. Manual Deleted successfully.";
                _response.Data = _mapper.Map<ManualDto>(_existingManual);


            }
            catch (Exception ex)
            {
                _response.MetaData.Succeeded = false;
                _response.Data = null;
                _response.MetaData.Message = "Error";
                _response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }
        #endregion
    }
}
