namespace eHandbook.Infrastructure.Services.ServiceResponder
{
    public class MetaData
    {
        public bool Succeeded { get; set; } = true;
        //This is a string property named Message.It’s intended to hold a message about the API response, such as an error message in case of a failure.
        public string? Message { get; set; } = null;
        //This is a string property named MyCustomErrorMessages.It’s intended to hold a personalized messsage about the API response.
        public List<string>? MyCustomErrorMessages { get; set; } = null;
    }
}