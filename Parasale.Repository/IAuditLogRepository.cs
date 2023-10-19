using Parasale.Models;
using System.Collections.Generic;

namespace Parasale.Repository
{
    public interface IAuditLogRepository
    {
        List<AuditLog> GetAuditLogsbyAdmin(string userid);
        List<AuditLog> GetAuditLogsbyManager(string userid);
    }
}