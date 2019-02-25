using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using QuestTracker.API.Entities;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Services;

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
            return Ok(this.AppContext.Folders);
        }

        [Route("", Name = "GetUserFolders")]
        public async Task<IHttpActionResult> GetUserFolders()
        {
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());

            if (user == null)
            {
                return NotFound();
            }

            var folders = user.Folders;

            return Ok(folders);
        }



    }
}
