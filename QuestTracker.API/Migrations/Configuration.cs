using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestTracker.API.Helpers;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QuestTracker.API.Infrastructure.AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "QuestTracker.API.AuthContext";
        }

        protected override void Seed(QuestTracker.API.Infrastructure.AuthContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AuthContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new AuthContext()));
            
            var user = new ApplicationUser()
            {
                UserName = "a_rueber@mailinator.com",
                Email = "a_rueber@mailinator.com",
                EmailConfirmed = true,
                FirstName = "A",
                LastName = "Rueber",
                JoinDate = DateTime.Now,
                PSK = OtpHelper.GenerateSharedPrivateKey(),
                IsActive = true
            };

            manager.Create(user, "MySuperP@ssword!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole {Name = "SuperAdmin"});
                roleManager.Create(new IdentityRole {Name = "Admin"});
                roleManager.Create(new IdentityRole {Name = "Owner"});
                roleManager.Create(new IdentityRole {Name = "User"});
            }

            var adminUser = manager.FindByName("a_rueber@mailinator.com");

            manager.AddToRoles(adminUser.Id, new string[] {"SuperAdmin", "Admin"});
        }
    }
}
