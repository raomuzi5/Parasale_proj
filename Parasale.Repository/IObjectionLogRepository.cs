using System;
using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface IObjectionLogRepository
    {
        List<ObjectionLog> GetObjectionLogs();
        List<ObjectionLog> getObjectionLogsById(int id);
        ObjectionLog GetObjectionLogsbyObjId(int ObjectionId);
        int GetObjectionLogsCountbyAdmin(string Id, bool IsCompleted);
        List<ObjectionLog> GetObjectionLogsbyUser(List<string> id, bool IsCompleted);
        List<ObjectionLog> GetObjectionLogsbyManagerUser(List<string> id);
        int GetObjectionLogsbyUser(string id, bool IsCompleted);
        List<ObjectionLog> GetObjectionLogs(int ObjectionId);
        List<ObjectionLog> GetObjectionLogs(string userId, bool IsCompleted);
        int GetObjectionLogsCount(string userId, bool IsCompleted);
        bool AllObjectionsCompleted(string userId);
        ObjectionLog getObjectionLogById(int id);
        List<ObjectionLog> AllObjectionsCompleted(string userId, DateTime date);
        int GetObjectionLogsCount(bool IsCompleted);
        List<AuditLog> AuditLogs();
        List<ObjectionLog> GetObjectionLogs(string id);
        List<AuditLog> AuditLogs(string userId, string ManagerId);
        List<AuditLog> UserAuditLogs(string id);
        List<ObjectionLog> GetObjectionLogsbyManager(List<string> id);
        List<ObjectionLog> GetObjectionLogsbyUser(string id);
        List<ObjectionLog> GetObjectionLogsbyObjId(List<int> id);
        List<AuditLog> AuditLogsbyAdmin(string id);


    }
}