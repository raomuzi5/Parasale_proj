using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface IInvitiesRepository
    {
        List<Invites> GetAllInvities(string id);
        List<Invites> GetAllInvities();
        Invites getInviteById(int id);
        Invites getInviteByGuid(string token);
        Invites getInviteByEmail(string email, string token);
        Invites getInviteByEmail(string email);
        List<InvitesByManager> GetAllInvitiesbyManager();
        List<InvitesByManager> GetAllInvitiesbyManager(string id);
        InvitesByManager getManagerInviteByGuid(string token);
        InvitesByManager getManagerInviteByEmail(string email,string token);
        InvitesByManager getManagerInviteByEmail(string email);
        InvitesByManager getManagerInviteById(int id);
    }
}