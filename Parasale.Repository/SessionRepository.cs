using Microsoft.EntityFrameworkCore;
using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class SessionRepository: ISessionRepository
    {
        private ParasaleDbContext _context;
        public SessionRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public UserSession GetSessionByID(int id)
        {
            return _context.userSessions.Include(p=>p.appUser).FirstOrDefault(x=>x.Id==id);
        }
        public int GetLastSessionID(string userid)
        {
            var a =_context.userSessions.Include(x => x.appUser).Where(x => x.appUser.Id == userid).OrderByDescending(x=>x.SessionID).FirstOrDefault();
            
            if(a!= null)
            {

            return a.SessionID;
            }
            else
            {
                //a.SessionID = 0;
                return 0;
            }
        }

       public List<SessionObjection> GetSessionObjection(int objId)
        {
            return _context.sessionObjections.Include(p => p.objection).Where(x => x.objection.Id == objId).ToList();
        }
    }

}
