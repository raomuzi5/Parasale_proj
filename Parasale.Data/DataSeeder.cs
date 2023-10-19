//using Microsoft.AspNetCore.Identity;
//using Parasale.Models;
//using System;
//using System.Threading.Tasks;

//namespace Parasale.Data
//{
//    public class DataSeeder
//    {
//        private UserManager<AppUser> _userManager;
//        private RoleManager<AppRole> _roleManager;
//        private ParasaleDbContext _dbcontext;
//        public static string ConnectionString;

//        public DataSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ParasaleDbContext context)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _dbcontext = context;
//        }
//        public async Task Seed()
//        {
//            var admin = await _userManager.FindByNameAsync("gigantic");
//            if (admin == null)
//            {
//                var user = new AppUser()
//                {
//                    Name = "Gigantic",
//                    Email = "gigantic@parasale.co",
//                    UserName = "gigantic"
//                };
//                var result = await _userManager.CreateAsync(user, "Gigantic1");
//                if (result.Succeeded)
//                {
//                    var Adminrole = await _roleManager.FindByNameAsync("Gigantic");
//                    if (Adminrole == null)
//                    {
//                        var newRole = new AppRole()
//                        {
//                            Name = "Gigantic",
//                            CreatedDate = DateTime.Today,
//                            Description = "Role for Quick Collections",
//                        };
//                        await _roleManager.CreateAsync(newRole);
//                    }

//                    var userToAssignRole = await _userManager.FindByNameAsync(user.UserName);
//                    await _userManager.AddToRoleAsync(userToAssignRole, "Gigantic");
//                }
//                var AdminRole = await _roleManager.FindByNameAsync("Admin");
//                if (AdminRole == null)
//                {
//                    var newRole = new AppRole()
//                    {
//                        Name = "Admin",
//                        CreatedDate = DateTime.Today,
//                        Description = "Role for Application Admin",
//                    };
//                    await _roleManager.CreateAsync(newRole);
//                }
//                var UserRole = await _roleManager.FindByNameAsync("User");
//                if (UserRole == null)
//                {
//                    var newRole = new AppRole()
//                    {
//                        Name = "User",
//                        CreatedDate = DateTime.Today,
//                        Description = "Role for Application User",
//                    };
//                    await _roleManager.CreateAsync(newRole);
//                }
//                var UserRoleManager = await _roleManager.FindByNameAsync("Manager");
//                if (UserRoleManager == null)
//                {
//                    var newRole = new AppRole()
//                    {
//                        Name = "Manager",
//                        CreatedDate = DateTime.Today,
//                        Description = "Role for Manage Users",
//                    };
//                    await _roleManager.CreateAsync(newRole);
//                }

//            }

//        }
//    }
//}
