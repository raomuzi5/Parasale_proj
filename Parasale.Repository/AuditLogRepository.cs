using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private ParasaleDbContext _context;
        public AuditLogRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public List<AuditLog> GetAuditLogsbyAdmin(string userid)
        {
            return _context.AuditLog.Where(a => a.UserId == userid).ToList();
        }
        public List<AuditLog> GetAuditLogsbyManager(string userid)
        {
            return _context.AuditLog.Where(a => a.ManagerId == userid).ToList();
        }

    }
}
