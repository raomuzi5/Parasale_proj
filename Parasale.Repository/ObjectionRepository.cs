using Microsoft.EntityFrameworkCore;
using Parasale.Data;
using Parasale.Models;
using System.Collections.Generic;
using System.Linq;

namespace Parasale.Repository
{
    public class ObjectionRepository : IObjectionRepository
    {
        private ParasaleDbContext _context;

        public ObjectionRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public List<Objection> GetAllbjections()
        {
            return _context.Objections.Include(p => p.User).Include(p=>p.collection).ToList();
        }

        public Objection GetObjection(string id)
        {
            return _context.Objections.Include(p => p.User).Include(p => p.collection).Where(x=>x.User.Id == id).FirstOrDefault() ;
        }

        public Objection GetObjection(int id)
        {
            return _context.Objections.Include(p=>p.User).Include(p=>p.collection).FirstOrDefault(p => p.Id == id);
        }
        
        public int GetObjectionsCount()
        {
            return _context.Objections.Count();
        }

        public List<Objection> getObjectionsbyCollection( int id)
        {
            return _context.Objections.Include(p=>p.User).Include(p=>p.collection).Where(p => p.collection.Id == id).ToList();
        }
        public List<Objection> getObjectionsbyCollections(List<int> id)
        {
            return _context.Objections.Include(p => p.User).Include(p => p.collection).Where(p => id.Contains(p.collection.Id)).ToList();//x => id.Contains(x.objection.Id))
        }
        public List<Objection> GetAllbjectionsFromManager(string managerUserId)
        {
            return _context.Objections.Where(p => p.User.Id == managerUserId).ToList();
        }
        public List<Objection> GetAllbjectionsFromManager(string managerUserId, string AdminID)
        {
            return _context.Objections.Where(p => p.User.Id == managerUserId || p.AssignedAdmin == AdminID).ToList();
        }
       
        public List<Objection> GetAllObjectionsByAdmin(string id)
        {
            return _context.Objections.Include(x=>x.collection).Include(x=>x.User).Where(p => p.AssignedAdmin == id).ToList();
        }
        public List<Objection> GetAllObjectionsWithnoCollections()
        {
            return _context.Objections.Include(p => p.User).Include(p => p.collection).Where(p=>p.collection.Id == null).ToList();
        }

        public Objection GetObjectionByID(int id)
        {
            return _context.Objections.Include(p => p.User).Include(p => p.collection).Where(p => p.Id == id).FirstOrDefault();
        }
        public List<Objection> GetAllObjectionsWithnoCollections(string id)
        {
            return _context.Objections.Include(p => p.User).Include(p => p.collection).Where(p => p.collection.Id == null && p.AssignedAdmin == id).ToList();
        }
        public List<Objection> GetAllbjectionsFromManagerWithnoCollection(string managerUserId)
        {
            return _context.Objections.Where(p => p.User.Id == managerUserId && p.collection==null).ToList();
        }
        public List <Objection> getObjectionByCollectionID(int id)
        {
            return _context.Objections.Where(p => p.collection.Id ==id).ToList();
        }

    }


    public class QuickObjectionRepository : IQuickObjectionRepository
    {
        private ParasaleDbContext _context;

        public QuickObjectionRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public QuickStartObjections GetAllObjectionbyId(int id)
        {
            return _context.qObjection.Include(p => p.User).Include(p=>p.collection).FirstOrDefault(x => x.Id == id);
        }
        public List<QuickStartObjections> GetAllObjectionbyCollectionId(int id)
        {
            return _context.qObjection.Include(p => p.User).Where(x => x.collection.Id == id).ToList();
        }
        public List<QuickStartObjections> GetAllObjectionbyCollectionIds(List<int> id)
        {
            return _context.qObjection.Include(p => p.User).Where(x => id.Contains(x.collection.Id)).ToList();
        }
        public List<QuickStartObjections> GetAllObjectionbyUser(string userId)
        {
            return _context.qObjection.Include(p => p.User).Where(x => x.AssignedAdmin ==userId).ToList();
        }
        public List<QuickStartObjections> GetAllObjections()
        {
            return _context.qObjection.ToList();
        }
        public List<QuickStartObjections> GetAllObjections(string id)
        {
            return _context.qObjection.Include(p => p.User).Where(x => x.User.Id == id).ToList();
        }

    }
}

