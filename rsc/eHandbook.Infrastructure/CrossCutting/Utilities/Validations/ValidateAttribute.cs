using System.ComponentModel.DataAnnotations;

namespace eHandbook.Infrastructure.CrossCutting.Utilities.Validations
{
    /// <summary>
    /// Creatng my own attribute.This class inherits from Attribute so it can be used as a sort of "tag" in our code. Conversions:
    /// Class name = AttributeName + Attribute. e.g ValidateAttribute where AttributeName = Validate.
    /// The attribute [Validate] signals the parameter that wll be validate by Filters.
    /// [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)] is for restricng the use of my custom attribute just on a single Parameter, otherwise I get compiler error.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class ValidateAttribute : Attribute
    {
    }

    [AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public class NotEmptyAttribute : ValidationAttribute
    {
        public const string DefaultErrorMessage = "The {0} field must not be empty";
        public NotEmptyAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            //NotEmpty doesn't necessarily mean required
            if (value is null)
            {
                return true;
            }

            switch (value)
            {
                case Guid guid:
                    return guid != Guid.Empty;
                default:
                    return true;
            }
        }
    }
}
