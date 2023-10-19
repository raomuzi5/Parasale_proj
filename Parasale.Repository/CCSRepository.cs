using Microsoft.EntityFrameworkCore;
using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class CCSRepository : ICCSRepository
    {
        private ParasaleDbContext _context;

        public CCSRepository(ParasaleDbContext context)
        {
            _context = context;
        }
        public CCS getCCsById(int id)
        {
            return _context.CCS.Include(p => p.appUser).Where(x => x.Id == id).FirstOrDefault();

        }
        public List<CCS> GetScorebyObjectionId(int id)
        {
            return _context.CCS.Include(p => p.appUser).Where(x => x.objection.Id == id).ToList();
        }
        public List<CCS> GetScorebyObjectionId(List<int> id)
        {
            return _context.CCS.Include(p => p.appUser).Where(x => id.Contains(x.objection.Id)).ToList();
        }
        public List<CCS> GetScorebyObjectionbyUser(string id)
        {
            return _context.CCS.Include(p => p.appUser).Where(x => x.appUser.Id == id).ToList();
        }

        public List<CCS> GetScorebyManagerUsers(List<string> id)
        {

            return _context.CCS.Include(p => p.objection).Include(p => p.appUser).Where(x => id.Contains(x.appUser.Id)).ToList();
        }
    }
}
