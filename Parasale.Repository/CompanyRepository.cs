using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private ParasaleDbContext _context;
        public CompanyRepository(ParasaleDbContext context)
        {
            _context = context;
        }

        public List<Company> GetAllCompanies()
        {
            return _context.Companies.ToList();
        }

        public Company GetByCompanyId(int id)
        {
            return _context.Companies.Where(x => x.Id == id).FirstOrDefault();
        }

        public List<Invites> GetAllInvities()
        {
            return _context.Invites.ToList();
        }
        public Invites getInviteById(int id)
        {
            return _context.Invites.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
