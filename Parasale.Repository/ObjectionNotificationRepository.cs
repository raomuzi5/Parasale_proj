using Microsoft.EntityFrameworkCore;
using Parasale.Data;
using Parasale.Models;
using System.Collections.Generic;
using System.Linq;

namespace Parasale.Repository
{
    public class ObjectionNotificationRepository : IObjectionNotificationRepository
    {
        private ParasaleDbContext _context;

        public ObjectionNotificationRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public List<ObjectionNotification> GetObjectionsPushedByManager(string userId, bool IsPracticed = false)
        {
            return _context.ObjectionNotifications.Include(p => p.Objection).Where(p => p.PushedToUserId == userId && p.IsCleared == IsPracticed).ToList();
        }
        public List<ObjectionNotification> GetObjectionsPushedByManagers(string userId)
        {
            return _context.ObjectionNotifications.Include(p => p.Objection).Where(p => p.PushedByUserId == userId).ToList();
        }
    }
}
