using Microsoft.EntityFrameworkCore;
using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parasale.Repository
{
    public class ObjectionLogRepository : IObjectionLogRepository
    {
        private ParasaleDbContext _context;

        public ObjectionLogRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public List<ObjectionLog> GetObjectionLogs()
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).ToList();
        }
        public List<ObjectionLog> GetObjectionLogs(string id)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(x=>x.AssignedAdmin == id).ToList();
        }
        public List<ObjectionLog> GetObjectionLogsbyUser(string id)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(x => x.AppUser.Id == id).ToList();
        }

        public List<ObjectionLog> GetObjectionLogsbyManagerUser(List<string> id)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(x =>id.Contains(x.AppUser.Id)).ToList();
        }
        public List<ObjectionLog> GetObjectionLogsbyUser(List<string> id, bool IsCompleted)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(x =>id.Contains(x.AppUser.Id) && x.IsCompleted == IsCompleted).ToList();
        }

        public List<ObjectionLog> GetObjectionLogsbyObjId(List<int> id)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(x => id.Contains(x.Objection.Id)).ToList();
        }
        public int GetObjectionLogsbyUser(string id, bool IsCompleted)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(x =>x.AppUser.Id == id && x.IsCompleted == IsCompleted).Count();
        }
        public List<ObjectionLog> GetObjectionLogsbyManager(List<string> id)
        {
            
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(x=>id.Contains(x.AppUser.Id)).ToList();
        }
        public int GetObjectionLogsCount(bool IsCompleted)
        {
            return _context.ObjectionLogs.Where(p => p.IsCompleted == IsCompleted).Count();
        }


        public int GetObjectionLogsCountbyAdmin(string Id,bool IsCompleted)
        {
            return _context.ObjectionLogs.Where(p => p.IsCompleted == IsCompleted && p.AssignedAdmin == Id).Count();
        }


        public List<ObjectionLog> GetObjectionLogs(string userId, bool IsCompleted)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Where(p => p.AppUser.Id == userId && p.IsCompleted == IsCompleted).ToList();
        }
        public ObjectionLog getObjectionLogById(int id)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p=>p.AppUser).Where(p => p.Id == id).FirstOrDefault();

        }
        public List <ObjectionLog> getObjectionLogsById(int id)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Include(p => p.AppUser).Where(p => p.Id == id).ToList();

        }
        public ObjectionLog GetObjectionLogsbyObjId(int ObjectionId)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Where(p => p.Objection.Id == ObjectionId).FirstOrDefault();
        }
        public List<ObjectionLog> GetObjectionLogs(int ObjectionId)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Where(p => p.Objection.Id == ObjectionId).ToList();
        }

        public int GetObjectionLogsCount(string userId, bool IsCompleted)
        {
            return _context.ObjectionLogs.Include(p => p.Objection).Where(p => p.AppUser.Id == userId && p.IsCompleted == IsCompleted).ToList().Count;
        }

        public bool AllObjectionsCompleted(string userId)
        {
            var objections = _context.ObjectionLogs.Where(p => p.AppUser.Id == userId &&
            p.ObjectionTime.Date == DateTime.Today.AddDays(-1).Date).ToList();
            bool IsAllObjectionsCompleted = false;
            if (objections != null && objections.Count() > 0)
            {
                IsAllObjectionsCompleted = objections.Any(p => p.IsCompleted == false);
                return !IsAllObjectionsCompleted;
            }
            return IsAllObjectionsCompleted;
        }

        public List<ObjectionLog> AllObjectionsCompleted(string userId, DateTime date)
        {
            return _context.ObjectionLogs.Where(p => p.AppUser.Id == userId &&
            p.ObjectionTime.Date == date).ToList();
        }

        public List<AuditLog> AuditLogsbyAdmin(string id)
        {
            return _context.AuditLog.Where(x => x.AdminId == id).ToList();
        }
        public List<AuditLog> AuditLogs()
        {
            return _context.AuditLog.ToList();
        }
        public List<AuditLog> UserAuditLogs(string id)
        {
            return _context.AuditLog.Where(x=>x.UserId == id).ToList();
        }

        public List<AuditLog> AuditLogs(string userId, string AdminId)
        {
            return _context.AuditLog.Where(x=>x.ManagerId == userId && x.AdminId == AdminId).ToList();
        }
    }
}
