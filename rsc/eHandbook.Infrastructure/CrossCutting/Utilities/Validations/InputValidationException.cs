using System.Runtime.Serialization;

namespace eHandbook.Infrastructure.CrossCutting.Utilities.Validations;

[Serializable]
internal class InputValidationException : Exception
{
    private readonly Dictionary<string, string[]> errors;

    public InputValidationException()
    {
    }

    public InputValidationException(Dictionary<string, string[]> errors)
    {
        this.errors = errors;
    }

    public InputValidationException(string? message) : base(message)
    {
    }

    public InputValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected InputValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}