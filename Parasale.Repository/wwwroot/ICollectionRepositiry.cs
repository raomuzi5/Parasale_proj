using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository.wwwroot
{
   public interface ICollectionRepositiry
    {
        List<Collection>GetCollec();
        List<ObjectionLog> GetObjectionLogs(int ObjectionId);
        List<ObjectionLog> GetObjectionLogs(string userId, bool IsCompleted);
        int GetObjectionLogsCount(string userId, bool IsCompleted);
        bool AllObjectionsCompleted(string userId);
        List<ObjectionLog> AllObjectionsCompleted(string userId, DateTime date);
        int GetObjectionLogsCount(bool IsCompleted);
    }
}
