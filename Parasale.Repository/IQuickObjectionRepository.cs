using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface IQuickObjectionRepository
    {
        List<QuickStartObjections> GetAllObjections();
        List<QuickStartObjections> GetAllObjections(string id);
        QuickStartObjections GetAllObjectionbyId(int id);
        List<QuickStartObjections> GetAllObjectionbyCollectionIds(List<int> id);
        List<QuickStartObjections> GetAllObjectionbyCollectionId(int id);
       List<QuickStartObjections> GetAllObjectionbyUser(string userId);
    }
}