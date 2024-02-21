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

    public class ManualDeletedNotification : INotification
    {
        public  string deleteResponse { get; set; }
    }

    public class ManualUpdateNotification : INotification
    {
        public string updateResponse { get; set; }
    }

}
