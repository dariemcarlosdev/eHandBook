using MediatR;
using Microsoft.Extensions.Logging;

namespace eHandbook.modules.ManualManagement.Application.CQRS.EventPublishNotifications
{
    /// <summary>
    /// Handler for manage request business logic sending emails when event happens(notification).
    /// /// It implement INotificationHandler<ManualAddedNotification> signifying it can handle this event.
    /// </summary>
    public sealed class EmailHandler : INotificationHandler<ManualCreatedNotification>
    {
        private readonly ILogger _logger;
        public EmailHandler(ILogger<EmailHandler> logger) => _logger = logger;

        public Task Handle(ManualCreatedNotification notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Email sent.");
            return Task.CompletedTask;
        }
    }
}
