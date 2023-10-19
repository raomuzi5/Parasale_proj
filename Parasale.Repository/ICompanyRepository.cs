using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface ICompanyRepository
    {
        List<Company> GetAllCompanies();
        List<Invites> GetAllInvities();
        Company GetByCompanyId(int id);
        Invites getInviteById(int id);
    }
}