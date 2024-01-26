using eHandbook.Core.Services.Common.ServiceResponder;
using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Queries.GetManual
{
    /// <summary>
    /// Query for retrieving the Person by ID. In CQRS, a query represents a request for data or information from the system without causing any changes to the system's state. 
    /// It is used to retrieve data or perform read-only operations.Queries can be designed and optimized specifically for read-intensive operations
    /// ,such as data retrieval, filtering, sorting, and aggregating.
    /// </summary>
    //internal sealed class GetManualByIdQuery : IRequest<ResponderService<ManualDto>>
    //{
    //    public Guid Id { get; set; }
    //}

    public sealed record GetManualByIdQueryRec(Guid Id) : IRequest<ResponderService<ManualDto>>;
}
