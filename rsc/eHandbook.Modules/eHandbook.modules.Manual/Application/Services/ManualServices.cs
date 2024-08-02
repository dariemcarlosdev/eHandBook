using AutoMapper;
using eHandbook.Core.Persistence.Abstractions;
using eHandbook.Infrastructure.Services.ServiceResponder;
using eHandbook.modules.ManualManagement.Application.Abstractions;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using eHandbook.modules.ManualManagement.CoreDomain.EntitiesModels;
using eHandbook.modules.ManualManagement.Infrastructure.Abstractions.Persistence;
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
        /// <summary>
        /// Create a new Manual in the database.
        /// </summary>
        /// <param name="manualCreateDtoRequest"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>This method return ApiResponseService type ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> AddNewManualAsync(ManualToCreateDto manualCreateDtoRequest, CancellationToken cancellationToken)

        {
            ApiResponseService<ManualDto> _response = new() { MetaData = new() };

            // this is a query to check if the manual already exist in the database.
            var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(e => e.Description == manualCreateDtoRequest.Description)!.FirstOrDefaultAsync(cancellationToken);

            try
            {

                //Check if Manual record exist.
                if (_existingManual != null)
                {
                    _response = ApiResponseService<ManualDto>.FailWithMessage("This Manual already Exist, hence cannot be created.");
                    return _response;
                }

                var _newManualEntity = new ManualEntity()
                {
                    Description = manualCreateDtoRequest.Description,
                    Path = manualCreateDtoRequest.Path
                };

                //Add new Manual Record
                if (!await _unitOfWork.GetRepository.CreateEntityAsync(_newManualEntity))
                {
                    _response = ApiResponseService<ManualDto>.FailWithMessage("Repository Error. A new manual could not be created.");
                    return _response;
                }

                else
                {
                    await _unitOfWork.SaveAsync(cancellationToken);
                    _response = ApiResponseService<ManualDto>.
                        SuccessWithMessage(_mapper.Map<ManualDto>(_newManualEntity), "Reponse OK!. Manual Created successfuly.");
                }

            }
            catch (Exception ex)
            {
                _response = ApiResponseService<ManualDto>.
                    FailWithCustomMessages("Error Response.", new List<string> { Convert.ToString(ex.Message) });
            }

            return _response;

        }

        //Status:implemented.Completed and Tested.
        /// <summary>
        /// Get Manual by Id Service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This method return ApiResponseService type ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> GetManualByIdAsync(Guid id, CancellationToken cancellationToken)
        {

            ApiResponseService<ManualDto> _response = new() { MetaData = new() };

            try
            {
                //check if manual record exists finding by condition using Id.
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(c => c.Id.Equals(id))!.FirstOrDefaultAsync(cancellationToken);

                if (_existingManual == null)
                {
                    _response = ApiResponseService<ManualDto>.FailWithMessage("Manual Not Found.");
                    return _response;
                }

                _response = ApiResponseService<ManualDto>.
                    SuccessWithMessage(_mapper.Map<ManualDto>(_existingManual), "Manual Found by ID.Reponse OK");

            }
            catch (Exception ex)
            {
                _response = ApiResponseService<ManualDto>.
                    FailWithCustomMessages("Error Response getting manual by Id", new List<string> { Convert.ToString(ex.Message) });
            }

            return _response;
        }


        //Status: Implemented.
        /// <summary>
        /// Get All Manuals Service method implementation.
        /// </summary>
        /// <param name=""></param>
        /// <returns> This method return ApiResponseService type ManualDto IEnumerable</returns>
        public async Task<ApiResponseService<IEnumerable<ManualDto>>> GetAllManualsAsync(CancellationToken cancellationToken)
        {
            ApiResponseService<IEnumerable<ManualDto>> _response = new() { MetaData = new() };
            try
            {
                var manuals = await _unitOfWork.GetRepository.GetAllEntitiesAsync(cancellationToken);

                if (manuals == null)
                {
                    _response = ApiResponseService<IEnumerable<ManualDto>>.
                        FailWithMessage("Manuals cannot be fetched.");
                    return _response;
                }

                _response = ApiResponseService<IEnumerable<ManualDto>>.
                    SuccessWithMessage(_mapper.Map<IEnumerable<ManualDto>>(manuals), "Manuals fetched OK.");
            }
            catch (Exception ex)
            {
                _response = ApiResponseService<IEnumerable<ManualDto>>.
                    FailWithCustomMessages("Error Response fetching all manuals.", new List<string> { Convert.ToString(ex.Message) });
            }

            return _response;
        }

        //Status: Implemented.Completed and Tested.
        /// <summary>
        /// Update Manual in DataBase Service and persist the change.
        /// </summary>
        /// <param name="manualToUpdateDtoRequest"></param>
        /// <returns>This method return ApiResponseService type ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> UpdateManualAsyn(ManualToUpdateDto manualToUpdateDtoRequest, CancellationToken cancellationToken)
        {
            //Create a empty response object.
            var _response = new ApiResponseService<ManualDto>();

            try
            {
                //Find the manual to be updated  and Check if Manual record exists.
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(m => m.Id.Equals(manualToUpdateDtoRequest.Id))!.FirstOrDefaultAsync(cancellationToken);

                //Check if Manual record exist.
                if (_existingManual == null)
                {

                    _response = ApiResponseService<ManualDto>.
                         FailWithCustomMessages("Manual do not exist, hence cannot be updated.", new List<string> { "The server cannot find the requested resource.Resource missing." });
                    return _response;

                }

                //this is where the manual properties are updated.
                _existingManual.Description = manualToUpdateDtoRequest.Description;
                _existingManual.Path = manualToUpdateDtoRequest.Path;

                //this checks if the manual was updated in the repository.
                if (!_unitOfWork.GetRepository.UpdateEntity(_existingManual/*, manualToUpdateDtoRequest)*/))
                {
                    _response = ApiResponseService<ManualDto>.
                        FailWithMessage("Repository Error updating Manual");
                    return _response;
                }

                //Update ManualRequest
                await _unitOfWork.SaveAsync(cancellationToken);

                _response = ApiResponseService<ManualDto>.
                    SuccessWithMessage(_mapper.Map<ManualDto>(_existingManual), "Manual Updated Ok");
            }
            catch (Exception ex)
            {
                _response = ApiResponseService<ManualDto>.
                    FailWithCustomMessages("Error updating Manual.", new List<string> { Convert.ToString(ex.Message) });
            }

            return _response;
        }

        //Status: Implemented. Completed and  Tested.
        /// <summary>
        /// Delete a Manual from database Service and persist the change.
        /// </summary>
        /// <param name="manualToDeleteDtoRequest"></param>
        /// <returns>This method return ApiResponseService type ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> HardDeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new();

            // this is a query to check if the manual already exist in the database.
            ManualEntity? _deleteManual = await _unitOfWork.GetRepository.FindEntityAsync(e => e.Id == manualToDeleteDtoRequest.Id, cancellationToken);

            try
            {
                //Check if Manual record exist.
                if (_deleteManual == null)
                {
                    _response = ApiResponseService<ManualDto>.
                        FailWithMessage("Manual not exist, hence it cannot be deleted.");
                    return _response;
                }

                if (!_unitOfWork.GetRepository.DeleteEntity(_deleteManual))
                {
                    _response = ApiResponseService<ManualDto>.
                        FailWithMessage("Repository Error deleting Manual");
                    return _response;
                }

                // if Manual was deteled. DeleteManual return true.
                await _unitOfWork.SaveAsync(cancellationToken);
                _response = ApiResponseService<ManualDto>.
                SuccessWithMessage(_mapper.Map<ManualDto>(_deleteManual), "Manual Deleted successfully.");

            }
            catch (Exception ex)
            {
                _response = ApiResponseService<ManualDto>.
                    FailWithCustomMessages("Error deleting Manual.", new List<string> { Convert.ToString(ex.Message) });
            }

            return _response;

        }

        //Status: Implemented. Completed and  Tested.
        /// <summary>
        /// Method Defined in ManualService Class.Delete Manual finding by Id from database and persist the change.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This method return ApiResponseService type ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> HardDeleteManualByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new() { MetaData = new() };

            try
            {
                //check if Manual exist.
                var _manualexist = await _unitOfWork.GetRepository.FindEntityByQueryable(manual => manual.Id == id)!.SingleOrDefaultAsync(cancellationToken);

                if (_manualexist == null)
                {
                    _response = ApiResponseService<ManualDto>.
                        FailWithMessage(
                        "Manual not exist, hence it cannot be deleted."
                        );
                    return _response;

                }

                if (!_unitOfWork.GetRepository.DeleteEntity(_manualexist))
                {
                    _response = ApiResponseService<ManualDto>.
                        FailWithMessage("Repository Error deleting Manual");
                    return _response;

                    //_response.MetaData.Succeeded = false;
                    //_response.MetaData.Message = "Repository Error deleting Manual";
                    //_response.Data = null;
                    //return _response;
                }

                // if Manual was deteled. DeleteManual return true.
                await _unitOfWork.SaveAsync(cancellationToken);
                _response = ApiResponseService<ManualDto>.
                    SuccessWithMessage(_mapper.Map<ManualDto>(_manualexist),
                    "Response Ok. Manual Deleted successfully."
                    );
            }
            catch (Exception ex)
            {

                _response = ApiResponseService<ManualDto>.
                    FailWithCustomMessages("Error deleting Manual.", new List<string> { Convert.ToString(ex.Message) });

                //_response.MetaData.Succeeded = false;
                //_response.Data = null;
                //_response.MetaData.Message = "Error deleting Manual.";
                //_response.MetaData.MyCustomErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }

            return _response;
        }

        ////Status: Implement. Completed and  Tested.
        /// <summary>
        /// Find a Manual and update Manual prop. Mark Entity prop. IsDeleted to tru, no hard delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>This method return ApiResponseService type ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> SoftDeleteManualByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new() { MetaData = new() };

            try
            {
                //check if record exist
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(manual => manual.Id == id)!.FirstOrDefaultAsync();

                //check if an existing manual was found.
                if (_existingManual == null)
                {
                    _response = ApiResponseService<ManualDto>.FailWithMessage("Manual not exist, hence it cannot be deleted.");
                    return _response;
                }

                //check if update was successful.
                if (!_unitOfWork.GetRepository.UpdateEntity(_existingManual))
                {
                    _response = ApiResponseService<ManualDto>.
                        FailWithMessage("Repository Error soft deleting Manual.");
                    return _response;
                }

                //this is where prop. IsDeleted is updated to true.
                _existingManual.IsDeleted = true;

                await _unitOfWork.SaveAsync(cancellationToken);
                _response = ApiResponseService<ManualDto>.
                    SuccessWithMessage(_mapper.Map<ManualDto>(_existingManual), "Response Ok. Manual Deleted successfully.");

            }
            catch (Exception ex)
            {
                _response = ApiResponseService<ManualDto>.
                    FailWithCustomMessages("Error deleting Manual.", new List<string> { Convert.ToString(ex.Message) });
            }
            return _response;
        }

        ////Status: Implement. Completed and  Tested.
        /// <summary>
        /// Find a giving Manual and if it's found, mark prop. IsDeleted to true.
        /// </summary>
        /// <param name="manualToDeleteDtoRequest"></param>
        /// <returns>This method return ApiResponseService type ManualDto</returns>
        public async Task<ApiResponseService<ManualDto>> SoftDeleteManualAsync(ManualToDeleteDto manualToDeleteDtoRequest, CancellationToken cancellationToken)
        {
            ApiResponseService<ManualDto> _response = new() { MetaData = new() };
            try
            {
                //check if an existing manual was found.
                var _existingManual = await _unitOfWork.GetRepository.FindEntityByQueryable(manual => manual.Id == manualToDeleteDtoRequest.Id)!.FirstOrDefaultAsync();

                //if existing manual was not found.
                if (_existingManual == null)
                {
                    _response = ApiResponseService<ManualDto>.FailWithMessage("Manual not exist, hence it cannot be deleted.");
                    return _response;
                }

                //here prop. IsDeleted is updated to true.
                _existingManual!.IsDeleted = true;

                //check if update was successful.
                if (!_unitOfWork.GetRepository.UpdateEntity(_existingManual))
                {
                    _response = ApiResponseService<ManualDto>.FailWithMessage("Repository Error soft deleting Manual.");
                    return _response;
                }

                await _unitOfWork.SaveAsync(cancellationToken);
                _response = ApiResponseService<ManualDto>.
                    SuccessWithMessage(_mapper.Map<ManualDto>(_existingManual), "Response Ok. Manual Deleted successfully.");

            }
            catch (Exception ex)
            {
                _response = ApiResponseService<ManualDto>.
                    FailWithCustomMessages("Error deleting Manual.", new List<string> { Convert.ToString(ex.Message) });
            }
            return _response;
        }

        #endregion
    }
}
