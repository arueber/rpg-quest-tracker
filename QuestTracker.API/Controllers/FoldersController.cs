using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Folders")]
    public class FoldersController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("all", Name = "GetAllFolders")]
        public IHttpActionResult GetAllFolders()
        {
            var roles = this.AppRoleManager.;

            return Ok(roles);
        }

        [Route("", Name = "GetUserFolders")]
        public async Task<IHttpActionResult> GetUserFolders()
        {
            ApplicationUser user = await this.AppUserManager.FindByNameAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            var folders = user.Folders;

            return Ok(folders);
        }

    }
}
