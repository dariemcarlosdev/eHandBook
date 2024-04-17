namespace eHandbook.Infrastructure.CrossCutting.Services.ServiceResponder
{
    /// <summary>
    /// Generic wrapper for web api response.       
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Data">Data as Response.</param>
    /// <param name="Success">Response success or not.</param>
    /// <param name="Message">Response Message.</param>
    /// <param name="Error">Response Error message.</param>
    /// <param name="MyCustomErrorMessages">Exception error messages.</param>
    public class ResponderService<T>
    {
        /// <summary>
        /// Generic wrapper for web api response.       
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; } = null;
        public string? Error { get; set; } = null;
        public List<string>? MyCustomErrorMessages { get; set; } = null;
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

