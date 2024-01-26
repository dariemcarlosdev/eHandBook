using FluentValidation;

namespace eHandbook.Infrastructure.CrossCutting.Utilities.Validations
{
    internal class ValidatorDescriptor
    {
        //init-only setter method assigns a value to the property or the indexer element only during object construction, enforces immutability, so that once the object is initialized, it can't be changed. 
        public required int ArgIndex { get; init; }
        public required Type ArgType { get; init; }
        //Fluent Validation validator to be applied.
        public required IValidator Validator { get; init; }
    }
}
