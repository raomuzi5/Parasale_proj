using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class InvitiesRepository : IInvitiesRepository
    {
        private ParasaleDbContext _context;
        public InvitiesRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public List<Invites> GetAllInvities()
        {
            return _context.Invites.ToList();
        }
        public List<Invites> GetAllInvities(string id)
        {
            return _context.Invites.Where(x=>x.AssignedAdmin == id).ToList();
        }
        public Invites getInviteByGuid(string token)
        {
            return _context.Invites.Where(x => x.Guid == token).FirstOrDefault();
        }
        public Invites getInviteByEmail(string email, string token)
        {
            return _context.Invites.Where(x => x.Email == email && x.Guid == token).FirstOrDefault();
        }
        public Invites getInviteByEmail(string email)
        {
            return _context.Invites.Where(x => x.Email == email).FirstOrDefault();
        }
        public Invites getInviteById(int id)
        {
            return _context.Invites.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<InvitesByManager> GetAllInvitiesbyManager()
        {
            return _context.GetInvitesByManagers.ToList();
        }
        public List<InvitesByManager> GetAllInvitiesbyManager(string id)
        {
            return _context.GetInvitesByManagers.Where(x => x.AssignedManager == id).ToList();
        }
        public InvitesByManager getManagerInviteByGuid(string token)
        {
            return _context.GetInvitesByManagers.Where(x => x.Guid == token).FirstOrDefault();
        }
        public InvitesByManager getManagerInviteByEmail(string email)
        {
            return _context.GetInvitesByManagers.Where(x => x.Email == email).FirstOrDefault();
        }
        public InvitesByManager getManagerInviteByEmail(string email, string token)
        {
            return _context.GetInvitesByManagers.Where(x => x.Email == email && x.Guid == token).FirstOrDefault();
        }
        public InvitesByManager getManagerInviteById(int id)
        {
            return _context.GetInvitesByManagers.Where(x => x.Id == id).FirstOrDefault();
        }

    }



}
