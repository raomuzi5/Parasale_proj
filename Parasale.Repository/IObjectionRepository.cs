using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface IObjectionRepository
    {
        List<Objection> GetAllbjections();
        Objection GetObjection(int id);
        Objection GetObjectionByID(int id);
        List<Objection> getObjectionsbyCollections(List<int> id);
        List<Objection> getObjectionByCollectionID(int id);
        List<Objection> GetAllbjectionsFromManager(string managerUserId);
        List<Objection> GetAllbjectionsFromManager(string managerUserId, string AdminID);
        List<Objection> GetAllbjectionsFromManagerWithnoCollection(string managerUserId);
        int GetObjectionsCount();
        Objection GetObjection(string id);
        List<Objection> getObjectionsbyCollection(int id);
        List<Objection> GetAllObjectionsWithnoCollections();
        List<Objection> GetAllObjectionsByAdmin(string id);
        List<Objection> GetAllObjectionsWithnoCollections(string id);
    }
}