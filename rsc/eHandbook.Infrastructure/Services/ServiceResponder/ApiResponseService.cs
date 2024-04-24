﻿using NLog.Layouts;
using NUnit.Framework;

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
        public bool Succeeded { get; set; } = true;
        //This is a string property named Message.It’s intended to hold a message about the API response, such as an error message in case of a failure.
        public string? Message { get; set; } = null;
        //This is a string property named MyCustomErrorMessages.It’s intended to hold a personalized messsage about the API response.
        public List<string>? MyCustomErrorMessages { get; set; } = null;

        /// <summary>
        /// This is a static method that creates a new instance of  ApiResponseService<T> with Succeeded set to false and Message set to the input string errorMessage.
        /// This method can be used to return a failure response from the API.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiResponseService<T> Fail(string message)
        { 
            return new ApiResponseService<T> { Succeeded = false, Message = message };
        }

        /// <summary>
        /// This is a static method that creates a new instance of ApiResponseService<T> with Succeeded set to true and Data set to the input data. 
        /// This method can be used to return a successful response from the API.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponseService<T> Success(T data)
        { 
            return new ApiResponseService<T> { Succeeded = true, Data = data };
        }
    }

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

