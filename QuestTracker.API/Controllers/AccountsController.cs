using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Models;

namespace QuestTracker.API.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        // POST api/Accounts/CreateUser
        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Username,
                Email = createUserModel.Email,
                FirstName = createUserModel.FirstName,
                LastName = createUserModel.LastName,
                JoinDate = DateTime.Now.Date,
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

            if (!addUserResult.Succeeded)
            {
                return GetErrorResult(addUserResult);
            }

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new {userId = user.Id, code = code }));
            await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account",
                "Please confirm your account by click <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [HttpPost]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string password = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "User Id, Password, and Code are required");
                return BadRequest(ModelState);
            }
            
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            // check for credentials before confirm email
            bool validCredentials = await this.AppUserManager.CheckPasswordAsync(user, password);
            if (validCredentials)
            {
                IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    return GetErrorResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "The password is incorrect.");
                return BadRequest(ModelState);
            }
        }

        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            // Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok();
            }
            return NotFound();
        }
    }
}
