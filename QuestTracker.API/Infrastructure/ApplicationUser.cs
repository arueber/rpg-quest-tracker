using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace QuestTracker.API.Infrastructure
{
    public class ApplicationUser: IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        public string PSK { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [MaxLength(255)]
        public string PhotoURL { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            // userIdentity.AddClaim(new Claim("PSK", PSK));
            return userIdentity;
        }
    }
}