using eHandbook.Infrastructure.Utilities.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Reflection;

namespace eHandbook.Infrastructure.Utilities.Filters
{
    /// <summary>
    /// This class exposes a factory function that can be passed to AddEndpointFilterFactory on a route group.
    /// </summary>
    public static class MyValidationGenericFactoryFilter
    {
        /// <summary>
        /// Fatory Function passed to AddEndpointFilterFactory on a  Enpoints route group. This factory method does the following:
        /// 1. Checks the endpoint delegate and looks for any arguments decorated with the[Validate] attribute
        /// 2. Attempts to resolve the appropriate Fluent Validation validator type e.g.IValidator<RegisterCustomerRequest>
        /// 3. Creates a descriptor that represents the argument type, its position and resolved validator
        /// 4. If any descriptors are returned, register an endpoint filter delegate that validates the endpoint arguments using the appropriate validators
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
        {
            IEnumerable<ValidatorDescriptor> validatorDescriptors = GetValidatorsArguments(context.MethodInfo, context.ApplicationServices);

            //If any descriptors are returned, register an endpoint filter delegate that validates the endpoint arguments using the appropriate validators.
            if (validatorDescriptors.Any())
            {
                return invocationContext => Validate(validatorDescriptors, next, invocationContext);
            }

            //pass-thu Filter

            return invocationContext => next(invocationContext);

        }

        /// <summary>
        /// Register an endpoint filter delegate that validates the endpoint arguments using the appropriate validators.
        /// </summary>
        /// <param name="validatorDescriptors"></param>
        /// <param name="next"></param>
        /// <param name="invocationContext"></param>
        /// <returns></returns>
        private static async ValueTask<object?> Validate(IEnumerable<ValidatorDescriptor> validatorDescriptors
            , EndpointFilterDelegate next
            , EndpointFilterInvocationContext invocationContext)
        {
            try
            {
                foreach (var validatorDescriptor in validatorDescriptors)
                {
                    var argument = invocationContext.Arguments[validatorDescriptor.ArgIndex];

                    if (argument is not null)
                    {
                        //here I am registering an endpoint filter delegate that validates the endpoint arguments using the appropriate validators.
                        var validationResult = await validatorDescriptor.Validator.ValidateAsync(
                            new ValidationContext<object>(argument)
                            );
                        if (!validationResult.IsValid)
                        {
                            return Results.ValidationProblem(validationResult.ToDictionary(),
                                statusCode: (int)HttpStatusCode.UnprocessableEntity,
                                detail: validationResult.ToString(),
                                type: "ValidationError",
                                title: "Invalid request"
                                );
                        }
                    }

                }
                return next.Invoke(invocationContext);
            }
            catch (Exception)
            {

                throw;
            }


        }

        /// <summary>
        /// Checks the endpoint delegate and looks for any arguments decorated with the [Validate] attribute.
        /// Attempts to resolve the appropriate Fluent Validation validator type e.g. IValidator&lt;GetManualByIdRequestValidator&gt;
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="applicationServices"></param>
        /// <returns>IEnumerable&lt;ValidatorDescriptor&gt;</returns>
        static IEnumerable<ValidatorDescriptor> GetValidatorsArguments(MethodInfo methodInfo, IServiceProvider applicationServices)
        {
            try
            {
                var parametersArray = methodInfo.GetParameters();
                var validatorDescriptorList = new List<ValidatorDescriptor>();
                for (int i = 0; i < parametersArray.Length; i++)
                {
                    var parameter = parametersArray[i];
                    if (parameter.GetCustomAttribute<ValidateAttribute>() is not null)
                    {
                        //here I am attempting to resolve the appropriate Fluent Validation validator.IValidator<GetManualByIdQueryRec>
                        Type validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                        // Note that FluentValidation validators needs to be registered as singleton.
                        //A Singleton class lifetime ensures that only one instance of a service is created and shared throughout the application's lifetime.
                        //This means that whenever a request for the service is made, the same instance is returned.
                        IValidator? fluentValidatorInstance = applicationServices.GetServices(validatorType) as IValidator;

                        if (fluentValidatorInstance is not null)
                        {
                            //yield statement in an iterator to provide the next value or signal the end of an iteration.
                            //yield return new ValidationDescriptor { ArgumentIndex = i, ArgumentType = parameter.ParameterType, Validator = validator };

                            //here a descriptor that represents the argument type, its position and resolved validator is created and added to de List validatorDescriptorList.
                            validatorDescriptorList.Add(new ValidatorDescriptor { ArgIndex = i, ArgType = parameter.ParameterType, Validator = fluentValidatorInstance });
                        }

                    }
                }
                return validatorDescriptorList;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
