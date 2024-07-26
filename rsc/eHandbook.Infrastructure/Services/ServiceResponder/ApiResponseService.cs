using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace eHandbook.Infrastructure.Services.ServiceResponder
{
    /// <summary>
    /// Generic wrapper for web api response.It represents a standard structure for API responses.      
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data">Data as Response.</param>
    /// <param name="Success">Response success or not.</param>
    /// <param name="Message">Response Message.</param>
    /// <param name="Error">Response Error message.</param>
    /// <param name="MyCustomErrorMessages">Exception error messages.</param>
    public class ApiResponseService<T>
    {
        //This is a property of type T named Data. It’s intended to hold the data returned by the API in case of a successful response.
        public T? Data { get; set; }
        //This is a boolean property named Succeeded.It indicates whether the API request was successful or not.
        public MetaData? MetaData { get; set; }

        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// This is a static method that creates a new instance of  ApiResponseService<T> with Succeeded set to false and Message set to the input string errorMessage.
        /// This method can be used to return a failure response from the API.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>
        /// This method, FailWithMessage, returns a new instance of the ApiResponseService<T> class with the Succeeded property set to false and the Message property set to the provided message. 
        /// It is used to create a failure response from the API. The Data property is set to the default value of type T, indicating that there is no data associated with the failure response.
        /// </returns>
        public static ApiResponseService<T> FailWithMessage(string message)
        {
            return new ApiResponseService<T> {
                Data = default,
                MetaData = new() { 
                    Message = message, 
                    Succeeded = false 
                } 
            };
        }

        ///--------------------------------------------------------------------------------------------------=
        ///<summary>
        ///This is a static method that creates a new instance of ApiResponseService<T> with Succeeded set to false and Message set to the input string errorMessage.
        ///This method can be used to return a failure response from the API.
        ///</summary>
        ///<param name="customErrorMessages"></param>
        ///<param name="errorMessage"></param>
        ///<return>
        ///This method returns a new instance of the ApiResponseService<T> class with the Succeeded property set to false, the Message property set to the provided errorMessage, and the MyCustomErrorMessages property set to the provided customErrorMessages list. 
        ///The Data property is set to the default value of type T, indicating that there is no data associated with the failure response.
        ///</return>
        public static ApiResponseService<T> FailWithCustomMessages(string errorMessage, List<string> customErrorMessages)
        {
            return new ApiResponseService<T> { 
                Data = default,
                MetaData = new () {
                    Succeeded = false,
                    Message = errorMessage,
                    MyCustomErrorMessages = customErrorMessages
                }
            };
        }


        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// This is a static method that creates a new instance of ApiResponseService<T> with Succeeded set to true and Data set to the input data. 
        /// This method can be used to return a successful response from the API.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A new instance of ApiResponseService<T> representing a successful response with the provided data.</returns>
        public static ApiResponseService<T> Success(T data)
        {
            return new ApiResponseService<T> { Data = data, MetaData = new () { Succeeded = true } };
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// This method creates a new instance of ApiResponseService<T> with the provided data and message, indicating a successful response.
        /// </summary>
        /// <param name="data">The data to be included in the response.</param>
        /// <param name="message">The message to be included in the response.</param>
        /// <returns>A new instance of ApiResponseService<T> representing a successful response with the provided data and message.</returns>
        public static ApiResponseService<T> SuccessWithMessage(T data, string message)
        {
            return new ApiResponseService<T> { Data = data, MetaData = new() { Succeeded = true, Message = message } };
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of ApiResponseService<T> with the provided success flag, indicating the success or failure of the API request.
        /// </summary>
        /// <param name="success">A boolean value indicating the success or failure of the API request.</param>
        /// <returns>A new instance of ApiResponseService<T> with the Succeeded flag set to the provided success value.</returns>
        public static ApiResponseService<T> IsSuccessFlagOnly(bool success)
        {
            return new ApiResponseService<T> { MetaData = new() { Succeeded = success } };
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// This method creates a new instance of ApiResponseService<T> with the provided data and message, indicating a successful response.
        /// </summary>
        /// <param name="data">The data to be included in the response.</param>
        /// <param name="message">The message to be included in the response.</param>
        /// <returns>A new instance of ApiResponseService<T> representing a successful response with the provided data and message.</returns>
        public static ApiResponseService<T> SuccessWithMessage(T data, string message)
        {
            return new ApiResponseService<T> { Data = data, MetaData = { Succeeded = true, Message = message } };
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of ApiResponseService<T> with the provided success flag, indicating the success or failure of the API request.
        /// </summary>
        /// <param name="success">A boolean value indicating the success or failure of the API request.</param>
        /// <returns>A new instance of ApiResponseService<T> with the Succeeded flag set to the provided success value.</returns>
        public static ApiResponseService<T> IsSuccessFlagOnly(bool success)
        {
            return new ApiResponseService<T> { MetaData = { Succeeded = success } };
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// This method creates a new instance of ApiResponseService<T> with the provided data and message, indicating a successful response.
        /// </summary>
        /// <param name="data">The data to be included in the response.</param>
        /// <param name="message">The message to be included in the response.</param>
        /// <returns>A new instance of ApiResponseService<T> representing a successful response with the provided data and message.</returns>
        public static ApiResponseService<T> SuccessWithMessage(T data, string message)
        {
            return new ApiResponseService<T> { Data = data, MetaData = { Succeeded = true, Message = message } };
        }

        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates a new instance of ApiResponseService<T> with the provided success flag, indicating the success or failure of the API request.
        /// </summary>
        /// <param name="success">A boolean value indicating the success or failure of the API request.</param>
        /// <returns>A new instance of ApiResponseService<T> with the Succeeded flag set to the provided success value.</returns>
        public static ApiResponseService<T> IsSuccessFlagOnly(bool success)
        {
            return new ApiResponseService<T> { MetaData = { Succeeded = success } };
        }
    }


    ///--------------------------------------------------------------------------------------------------
    /// <summary>
    /// Records definition instead of Class for ResposeService.It's not gonna be used for now.Maybe later on.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data">Data as Response.</param>
    /// <param name="Success">Response success or not.</param>
    /// <param name="Message">Response Message.</param>
    /// <param name="Error">Response Error message.</param>
    /// <param name="MyCustomErrorMessages">Exception error messages.</param>
    public record ServiceResponseRecord<T>(T? Data, bool Success = true, string? Message = null, string? Error = null, List<string>? MyCustomErrorMessages = null);
}

