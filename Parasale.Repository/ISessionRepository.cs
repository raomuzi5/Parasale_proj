using Parasale.Models;
using System.Collections.Generic;

namespace Parasale.Repository
{
    public interface ISessionRepository
    {
         UserSession GetSessionByID(int id);
        int GetLastSessionID(string userid);
        List <SessionObjection> GetSessionObjection(int objId);
    }
}