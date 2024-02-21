using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Commands.DeleteManualById
{
    internal sealed record DeleteManualByIdCommand(Guid ManualGuid) : IRequest<ResponderService<ManualDto>>;
}
