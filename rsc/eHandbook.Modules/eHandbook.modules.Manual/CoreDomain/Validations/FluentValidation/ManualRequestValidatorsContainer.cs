using Azure.Core;
using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.CoreDomain.Validations.FluentValidation
{
    public class ManualRequestValidatorsContainer
    {
        /// <summary>
        /// Each validator can contain a whole lot of Strongly Typed validation logic around your models.
        /// As the name suggest the validation style is fluent, meaning you can chain all the validation rules together.
        /// </summary>
        public partial class GetManualByIdRequestValidator : AbstractValidator<GetManualByIdQuery>
        {
            public GetManualByIdRequestValidator()
            {
                RuleFor(request => request.Id).NotEmpty().WithMessage("dsds");

            }
        }

    }
}
