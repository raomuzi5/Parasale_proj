using Microsoft.EntityFrameworkCore;
using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class CollectionRepository : ICollectionRepository
    {
        private ParasaleDbContext _context;

        public CollectionRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public List<Collection> GetAllCollections()
        {
            return _context.Collections.ToList();
        }
        public List<Collection> GetAllCollections(string id)
        {
            return _context.Collections.Include(p=>p.appUser).Where(x=>x.appUser.Id == id).ToList();
        }
        public List<Collection> GetAllCollections(string id, string adminId)
        {
            return _context.Collections.Include(p => p.appUser).Where(x => x.appUser.Id == id || x.AssignedAdmin == adminId).ToList();
        }
        public List<AssignedCollection> GetAllAssignedCollections(string id)
            {
            return _context.AssignedCollections.Include(p => p.appUser).Include(p => p.collection).Where(x => x.appUser.Id == id).ToList();
        }

        public List<AssignedCollection> GetAllAssignedCollections()
        {
            return _context.AssignedCollections.Include(p => p.appUser).Include(p => p.collection).ToList();
        }

        public List<AssignedCollection> GetAllAdminCollections( string id)
        {
            return _context.AssignedCollections.Include(p => p.appUser).Include(p => p.collection).Where(o=>o.AssignedAdmin== id).ToList();
        }
        public List<AssignedCollection> GetAssignedCollections(int id)
        {
            return _context.AssignedCollections.Include(p => p.appUser).Include(p => p.collection).Where(x => x.CollectionId == id).ToList();
        }

        public AssignedCollection GetAssignedCollections(int id, string userid)
        {
            return _context.AssignedCollections.Include(p => p.appUser).Include(p => p.collection).Where(x => x.CollectionId == id && x.UserId == userid).FirstOrDefault();
        }
        public Collection GetAllCollectionbyId(int id)
        {
            return _context.Collections.Include(p=>p.appUser).FirstOrDefault(x => x.Id == id);
        }
        public List<AssignedCollection> GetAllAssignedCollections(List<string> id)
        {
            return _context.AssignedCollections.Include(p => p.appUser).Include(p => p.collection).Where(x => id.Contains(x.appUser.Id)).ToList();
        }


        public Collection GetCollection(string id)
        {
            return _context.Collections.Include(p => p.appUser).Where(x => x.appUser.Id == id).FirstOrDefault();
        }
        public List<Collection> GetAdminManagerCollections(string id)
        {
            return _context.Collections.Include(p => p.appUser).Where(x => x.AssignedAdmin == id).ToList();
        }
    }
    public class QuickCollectionRepository : IQuickCollectionRepository
    {
        private ParasaleDbContext _context;

        public QuickCollectionRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public QuickCollection GetAllCollectionbyId(int id)
        {
            return _context.qCollections.Include(p => p.appUser).FirstOrDefault(x => x.Id == id);
        }
        public List<QuickCollection> GetAllCollectionbyAdmin(string userId)
        {
            return _context.qCollections.Include(p => p.appUser).Where(x => x.AssignedAdmin == userId).ToList(); ;
        }

        public List<QuickCollection> GetAllCollections()
        {
            return _context.qCollections.ToList();
        }
        public List<QuickCollection> GetAllCollections(string id)
        {
            return _context.qCollections.Include(p => p.appUser).Where(x => x.appUser.Id == id).ToList();
        }
        public QuickCollection getCollection(string id)
        {
            return _context.qCollections.Include(p => p.appUser).Where(x => x.appUser.Id == id).FirstOrDefault();

        }

    }
}
