using Mafiator.Service.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts;

public interface INotificationService
{
    Task<bool> CreateOrUpdateInstallationAsync(DeviceInstallation deviceInstallation, CancellationToken token);
    Task<bool> DeleteInstallationByIdAsync(string installationId, CancellationToken token);
    Task<bool> RequestNotificationAsync(NotificationRequest notificationRequest, CancellationToken token);
}