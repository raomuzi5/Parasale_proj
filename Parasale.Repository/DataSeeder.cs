using Microsoft.AspNetCore.Identity;
using Parasale.Data;
using Parasale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parasale.Repository
{
    public class DataSeeder
    {
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;
        private ParasaleDbContext _dbcontext;
        private IRepositoryWrapper _repository;
        public static string ConnectionString;

        public DataSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ParasaleDbContext context, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbcontext = context;
            _repository = repository;
        }
        public async Task Seed()
        {
            var admin = await _userManager.FindByNameAsync("gigantic");
            if (admin == null)
            {
                var user = new AppUser()
                {
                    Name = "Gigantic",
                    Email = "gigantic@parasale.co",
                    UserName = "gigantic"
                };
                var result = await _userManager.CreateAsync(user, "Gigantic1");
                if (result.Succeeded)
                {
                    var Adminrole = await _roleManager.FindByNameAsync("Gigantic");
                    if (Adminrole == null)
                    {
                        var newRole = new AppRole()
                        {
                            Name = "Gigantic",
                            CreatedDate = DateTime.Today,
                            Description = "Role for Quick Collections",
                        };
                        await _roleManager.CreateAsync(newRole);
                    }

                    var userToAssignRole = await _userManager.FindByNameAsync(user.UserName);
                    await _userManager.AddToRoleAsync(userToAssignRole, "Gigantic");
                }
                var AdminRole = await _roleManager.FindByNameAsync("Admin");
                if (AdminRole == null)
                {
                    var newRole = new AppRole()
                    {
                        Name = "Admin",
                        CreatedDate = DateTime.Today,
                        Description = "Role for Application Admin",
                    };
                    await _roleManager.CreateAsync(newRole);
                }
                var UserRole = await _roleManager.FindByNameAsync("User");
                if (UserRole == null)
                {
                    var newRole = new AppRole()
                    {
                        Name = "User",
                        CreatedDate = DateTime.Today,
                        Description = "Role for Application User",
                    };
                    await _roleManager.CreateAsync(newRole);
                }
                var UserRoleManager = await _roleManager.FindByNameAsync("Manager");
                if (UserRoleManager == null)
                {
                    var newRole = new AppRole()
                    {
                        Name = "Manager",
                        CreatedDate = DateTime.Today,
                        Description = "Role for Manage Users",
                    };
                    await _roleManager.CreateAsync(newRole);
                }

            }
            var VoiceOnBoardings = _repository.VoiceOnboarding.FirstOrDefaultVB();
            if (VoiceOnBoardings == null)
            {
                try
                {
                    var VBData = new VoiceOnBoarding()
                    {
                        Step = 1,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"Now   that   this   is   complete.   Please   state   what   you   would   like   to   learn   first   from   the   following   list: <ul> <li><a href='#' id='14' class='goToStep'>Onboarding users and creating Teams</a></li> <li><a href='#' id='2' class='goToStep' data-ele='.practiceObjections'>Practicing objections</a></li> <li><a href='#' id='27' class='goToStep'>Reporting</a></li> <li><a href='#' id='42' class='goToStep'>Audit logs</a></li></ul>",
                        Message = "Now   that   this   is   complete.   Please   state   what   you   would   like   to   learn   first   from   the   following   list:    1.Onboarding   users   and   creating   Teams  2.Practicing   objections   3.Reporting  4.Audit   logs",
                        Title = "Welcome",
                        Element = ".navbar",
                        Position = "Left",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };
                    var VBData2 = new VoiceOnBoarding()
                    {
                        Step = 0,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"“Welcome   to   Parasale.  My   name   is   Para   I   am   your   self   onboarding   agent.   We   are   so   glad   to   have   you.   Please   click  grant   permission   to   grant   permission   for   our   program  t o   use   your   microphone",
                        Message = "Welcome   to   Parasale.  My   name   is   Para   I   am   your   self   onboarding   agent.   We   are   so   glad   to   have   you.   Please   click  grant   permission   to   grant   permission   for   our   program  t o   use   your   microphone",
                        Title = "Welcome",
                        Element = ".navbar",
                        Position = "Left",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };

                    await _repository.ParasaleRepository.Add(VBData);
                    await _repository.ParasaleRepository.Add(VBData2);
                    await _repository.ParasaleRepository.Save();
                    var VBData3 = new VoiceOnBoarding()
                    {
                        ParentVOBId = VBData.Id,
                        Step = 2,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"Great. Let’s talk about practicing objections. Practicing   objections   involves   three   things, which   would   you   like   to   start with   first ?.",
                        Message = "Great.   Let’s   talk   about  practicing   objections.  P racticing   objections   involves   three   things,   which   would   you   like   to   start  with   first?   1.Installing   quick   start   collections  2.Building   collections   and   objections   3.Practicing   objections",
                        Title = "Practicing Objections",
                        Element = ".practiceObjections",
                        Position = "right",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };
                    _repository.ParasaleRepository.Add(VBData3);
                    await _repository.ParasaleRepository.Save();

                    var VBData4 = new VoiceOnBoarding()
                    {
                        ParentVOBId = VBData3.Id,
                        Step = 3,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"“Fantastic.   Let’s   work  learning   how   to   install  q uick   start   collections.   Please   click   on   the    manage   collections   tab   on   the  left   side   navigation  b ar",
                        Message = "“Fantastic.   Let’s   work  learning   how   to   install  q uick   start   collections.   Please   click   on   the    manage   collections   tab   on   the  left   side   navigation  b ar",
                        Title = " Installing   quick   start   collections ",
                        Element = ".manageCollections",
                        Position = "right",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };
                    var VBData5 = new VoiceOnBoarding()
                    {
                        ParentVOBId = VBData3.Id,
                        Step = 4,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"That   was   quick.   Now   please   direct   your   eyes   to   the   second   list   on  this   page   that   says  q uick   start   collections.   Now,   let   me   explain   what   a   quick   start   collection   is.   A  quick   start   collection  i s   an   easy   and   automatically   set   up   group   of   collections   you   can   push   to  users,   so   they   can   begin   practicing   with   Parasale   in   a   matter   of   moments.   The   way   to   install   a  quick   start   collection  i s   by   clicking   on   the   blue   arrow   shown   next   to   the   collection   name.   Please  click   on   any   blue   arrows   you   would   like  t o   add   to   your   collections",
                        Message = "That   was   quick.   Now   please   direct   your   eyes   to   the   second   list   on  this   page   that   says  q uick   start   collections.   Now,   let   me   explain   what   a   quick   start   collection   is.   A  quick   start   collection  i s   an   easy   and   automatically   set   up   group   of   collections   you   can   push   to  users,   so   they   can   begin   practicing   with   Parasale   in   a   matter   of   moments.   The   way   to   install   a  quick   start   collection  i s   by   clicking   on   the   blue   arrow   shown   next   to   the   collection   name.   Please  click   on   any   blue   arrows   you   would   like  t o   add   to   your   collections",
                        Title = " Building   collections   and   objections ",
                        Element = ".manageCollections",
                        Position = "right",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };
                    await _repository.ParasaleRepository.Add(VBData4);
                    await _repository.ParasaleRepository.Add(VBData5);
                    await _repository.ParasaleRepository.Save();

                    var VBData7 = new VoiceOnBoarding()
                    {
                        ParentVOBId = VBData.Id,
                        Step = 5,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"Onboarding   users   and   creating   Teams ",
                        Message = "Onboarding   users   and   creating   Teams ",
                        Title = "Onboarding   users   and   creating   Teams ",
                        Element = ".manageTeamsAndUsers",
                        Position = "right",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };
                    var VBData8 = new VoiceOnBoarding()
                    {
                        ParentVOBId = VBData.Id,
                        Step = 6,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"Reporting ",
                        Message = "Reporting ",
                        Title = "Reporting ",
                        Element = ".reports",
                        Position = "right",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };
                    var VBData9 = new VoiceOnBoarding()
                    {
                        ParentVOBId = VBData.Id,
                        Step = 7,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"Audit logs",
                        Message = "Audit logs",
                        Title = "Audit logs",
                        Element = ".auditlogs",
                        Position = "right",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };

                    await _repository.ParasaleRepository.Add(VBData7);
                    await _repository.ParasaleRepository.Add(VBData8);
                    await _repository.ParasaleRepository.Add(VBData9);
                    await _repository.ParasaleRepository.Save();
                    var VBData10 = new VoiceOnBoarding()
                    {
                        ParentVOBId = VBData3.Id,
                        Step = 8,
                        SubStep = 0,
                        IsStartupPopUp = false,
                        Description = $"'Great. Let’s talk about practicing objections. Practicing   objections   involves   three   things, which   would   you   like   to   start with   first ?<ul><li><a href='#' id='3' class='goToStep'>Installing Quick Start Collections.</a></li><li><a href='#' id='6' class='goToStep'>Building collections and objections.</a></li><li><a href='#' id='2' class='goToStep'>Practicing objections</a></li></ul>",
                        Message = "Great.   Let’s   talk   about  practicing   objections.  P racticing   objections   involves   three   things,   which   would   you   like   to   start  with   first?   1.Installing   quick   start   collections  2.Building   collections   and   objections   3.Practicing   objections",
                        Title = "Practicing Objections",
                        Element = ".practiceObjections",
                        Position = "right",
                        IsAdmin = true,
                        IsManager = true,
                        IsUser = true,
                        IsNextExist = true,
                        IsPreviousExist = false,
                    };
                    await _repository.ParasaleRepository.Add(VBData10);
                    await _repository.ParasaleRepository.Save();
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
