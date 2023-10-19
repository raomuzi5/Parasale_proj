using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class UserRepository : ParasaleRepository, IUserRepository
    {
        private ParasaleDbContext _context;

        public UserRepository(ParasaleDbContext context)
            : base(context)
        {
            _context = context;
        }

        public AppUser GetUser(string id)
        {
            return _context.Users.FirstOrDefault(p => p.Id == id);
        }

        public List<AppUser> GetManagersByAdmin(string id)
        {
            return _context.Users.Where(x => x.IsManager == true && x.AssginedAdmin == id).ToList();
        }

        public List<AppUser> GetUsersByAdmin(string id)
        {
            return _context.Users.Where(x=>x.AssginedAdmin == id).ToList();
        }

        public AppUser GetUsersByEmail(string email)
        {
            return _context.Users.Where(x => x.Email == email).FirstOrDefault();
        }
        public List<AppUser> GetUsers()
        {
            return _context.Users.ToList();
        }
        public List<AppUser> GetManagers()
        {
            return _context.Users.Where(x=>x.IsManager == true).ToList();
        }

        public List<AppUser> GetUsers(bool lockOut)
        {
            if (lockOut)
                return _context.Users.Where(x => x.LockoutEnd > DateTime.Now).ToList();
            else
                return _context.Users.Where(x => x.LockoutEnd !=null).ToList();
        }
        public int GetUsersCount()
        {
            return _context.Users.Count();
        }

        public string GetUsersCount(string id)
        {
            return _context.Users.Where(x=>x.AssginedAdmin == id && x.IsCompanyAdmin != true).Count().ToString();
        }
        public bool CheckUser()
        {
            return _context.Users.Any();
        }
        public List<AppUser> GetUsersUnderManager(string managerUserId)
        {
            return _context.Users.Where(p => p.AssignedManager == managerUserId && p.IsManager != true).ToList();
        }
    }
}
