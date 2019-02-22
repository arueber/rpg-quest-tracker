using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using QuestTracker.API.Filters;

namespace QuestTracker.API.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : BaseApiController
    {
        [Authorize(Users = "Admin")]
        [TwoFactorAuthorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(this.AuthRepository.GetAllRefreshTokens());
        }

        [Authorize(Users = "Admin")]
        [TwoFactorAuthorize]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await this.AuthRepository.RemoveRefreshTokenAsync(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.AuthRepository.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
