using System.Collections.Generic;
using System.Threading.Tasks;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface ICollectionRepository
    {
        Collection GetAllCollectionbyId(int id);
        List<Collection> GetAllCollections();
        List<Collection> GetAllCollections(string id);
        Collection GetCollection(string id);
        List<AssignedCollection> GetAllAssignedCollections(string id);
        List<AssignedCollection> GetAllAdminCollections(string id);
        List<AssignedCollection> GetAssignedCollections(int id);
        List<Collection> GetAllCollections(string id, string adminId);
        List<AssignedCollection> GetAllAssignedCollections();
        AssignedCollection GetAssignedCollections(int id, string userid);
        List<AssignedCollection> GetAllAssignedCollections(List<string> id);
        List<Collection> GetAdminManagerCollections(string id);

    }
    public interface IUserHistory
    {
        UserHistory GetLastUserHistory(string userid);
    }
    public interface IVoiceOnboardingData
    {
        VoiceOnBoarding GetVoiceOnBoardingByUserId(string userid);
        IEnumerable<VoiceOnBoarding> GetVoiceOnBoardings();
        VoiceOnBoarding FirstOrDefaultVB();
    }
    public interface IQuickCollectionRepository
    {
        QuickCollection GetAllCollectionbyId(int id);
        List<QuickCollection> GetAllCollections();
        List<QuickCollection> GetAllCollections(string id);
        QuickCollection getCollection(string id);
        //QuickCollection GetAllCollectionbyAdmin(string userId);
        List<QuickCollection> GetAllCollectionbyAdmin(string userId);

    }
}