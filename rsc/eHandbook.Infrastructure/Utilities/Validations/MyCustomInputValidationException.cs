using FluentValidation;
using FluentValidation.Results;
using System.Runtime.Serialization;

namespace eHandbook.Infrastructure.Utilities.Validations
{
    [Serializable]
    internal class MyCustomInputValidationException : ValidationException
    {
        public MyCustomInputValidationException(string message) : base(message)
        {
        }

        public MyCustomInputValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
        {
        }

        public MyCustomInputValidationException(string message, IEnumerable<ValidationFailure> errors) : base(message, errors)
        {
        }

        public MyCustomInputValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MyCustomInputValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage) : base(message, errors, appendDefaultMessage)
        {
        }
    }
}