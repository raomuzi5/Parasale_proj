using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface ICCSRepository
    {
        List<CCS> GetScorebyObjectionId(int id);
        List<CCS> GetScorebyObjectionbyUser(string id);
        List<CCS> GetScorebyManagerUsers(List<string> id);
        List<CCS> GetScorebyObjectionId(List<int> id);
        CCS getCCsById(int id);
    }
}