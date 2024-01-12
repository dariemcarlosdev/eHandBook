using eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual;
using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.CoreDomain.Validations
{
    public class GetManualByIdRequestValidator : AbstractValidator<GetManualByIdQuery>
    {
        public GetManualByIdRequestValidator() 
        { 
            RuleFor(request => request.Id).NotEmpty().WithMessage("ManualId is required.");
            //RuleFor(manual => manual.Description).NotNull().NotEmpty();
            //RuleFor(manual => manual.Path).NotNull().NotEmpty();
        }
        
    }
}
