using eHandbook.modules.ManualManagement.CoreDomain.DTOs.Manual;
using MediatR;

namespace eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications
{
    /// <summary>
    /// ManualCreatedNotification implement INotification
    /// </summary>
    public class ManualCreatedNotification : INotification
    {
        public ManualDto manual { get; set; }
    }

}
