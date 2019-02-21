using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestTracker.API.Entities;

namespace QuestTracker.API.Infrastructure
{
    public class ApplicationUser: IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        public int Revision { get; set; }

        [Required]
        public string PSK { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [MaxLength(255)]
        public string PhotoURL { get; set; }

        public virtual ICollection<Folder> Folders { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        public virtual ICollection<Item> AssignedItems { get; set; }

        public virtual ICollection<Item> CompletedItems { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            
            return userIdentity;
        }
    }

    public class CustomUserRole: IdentityUserRole<int> { }
    public class CustomUserClaim: IdentityUserClaim<int> { }
    public class CustomUserLogin: IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole,
        CustomUserClaim>
    {
        public CustomUserStore(ApplicationContext context): base(context) { }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationContext context) : base(context) { }
    }
}