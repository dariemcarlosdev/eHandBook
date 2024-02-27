using eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace eHandbook.modules.ManualManagement.Application.CQRS.Handlers
{
    /// <summary>
    /// Handler for manage request business logic auditing logs when event happens(notification).
    /// It implement INotificationHandler<ManualAddedNotification> signifying it can handle this event.
    /// </summary>
    public class AuditLogHandler : INotificationHandler<ManualCreatedNotification>
    {
        private readonly ILogger<AuditLogHandler> _logger;

        public AuditLogHandler(ILogger<AuditLogHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(ManualCreatedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Audit logs added.");
            return Task.CompletedTask;
        }
    }
}
