namespace AccommodationService.Notification;

public interface INotificationSenderService
{
    void Send(NotificationPayload payload);
}
