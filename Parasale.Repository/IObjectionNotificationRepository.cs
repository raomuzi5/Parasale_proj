using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface IObjectionNotificationRepository
    {
        List<ObjectionNotification> GetObjectionsPushedByManager(string userId, bool IsPracticed = false);
        List<ObjectionNotification> GetObjectionsPushedByManagers(string userId);
    }
}