using System.Web;
using Base32;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OtpSharp;
using QuestTracker.API.Helpers;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QuestTracker.API.Infrastructure.ApplicationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "QuestTracker.API.AuthContext";
        }

        protected override void Seed(QuestTracker.API.Infrastructure.ApplicationContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole,
                CustomUserClaim> (new ApplicationContext()));

            var roleManager = new ApplicationRoleManager(new CustomRoleStore(new ApplicationContext()));

            var user = new ApplicationUser()
            {
                UserName = "a_rueber@mailinator.com",
                Email = "a_rueber@mailinator.com",
                EmailConfirmed = true,
                FirstName = "A",
                LastName = "Rueber",
                JoinDate = DateTime.Now,
                Revision = 1,
                PSK = OtpHelper.GenerateSharedPrivateKey(),
                TwoFactorEnabled = true,
                IsActive = true
            };

            manager.Create(user, "MySuperP@ssword!");

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new CustomRole {Name = "SuperAdmin"});
                roleManager.Create(new CustomRole { Name = "Admin"});
                roleManager.Create(new CustomRole { Name = "Owner"});
                roleManager.Create(new CustomRole { Name = "User"});
            }

            var adminUser = manager.FindByName("a_rueber@mailinator.com");

            manager.AddToRoles(adminUser.Id, new string[] {"SuperAdmin", "Admin"});
        }
    }
}
