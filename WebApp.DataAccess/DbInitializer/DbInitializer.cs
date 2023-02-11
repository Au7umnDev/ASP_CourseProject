using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;
using WebApp.Utility;

namespace WebApp.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if(_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User_Company)).GetAwaiter().GetResult();
                //create admin if there were no roles
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "allyxan1337@gmail.com",
                    Email = "allyxan1337@gmail.com",
                    Name = "Глебушек",
                    PhoneNumber = "11231313111",
                    StreetAddress = "123 Ave",
                    State = "IL",
                    PostalCode = "123312",
                    City = "Chicago"
                },"Admin123*").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "allyxan1337@gmail.com");

                _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
