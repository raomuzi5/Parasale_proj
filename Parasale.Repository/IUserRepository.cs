using System.Collections.Generic;
using Parasale.Models;

namespace Parasale.Repository
{
    public interface IUserRepository
    {
        List<AppUser> GetUsers();
        int GetUsersCount();
       AppUser GetUsersByEmail(string email);
        string GetUsersCount(string id);
        List<AppUser> GetUsersUnderManager(string managerUserId);
        AppUser GetUser(string id);
        bool CheckUser();
        List<AppUser> GetManagers();
        List<AppUser> GetUsers(bool lockOut);
        List<AppUser> GetManagersByAdmin(string id);
        List<AppUser> GetUsersByAdmin(string id);


    }
}