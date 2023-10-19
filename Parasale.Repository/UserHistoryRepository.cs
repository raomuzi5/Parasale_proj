using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class UserHistoryRepository : IUserHistory
    {
        private ParasaleDbContext _context;
        public UserHistoryRepository(ParasaleDbContext context)
        {
            _context = context;
        }
        public UserHistory GetLastUserHistory(string UserId)
        {
            return _context.UserHistories.Where(a => a.UserId == UserId).LastOrDefault();
        }

      
    }
}
