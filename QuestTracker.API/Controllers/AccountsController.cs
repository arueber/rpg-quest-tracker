﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using QuestTracker.API.Filters;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Models;
using QuestTracker.API.Helpers;

namespace QuestTracker.API.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [Route("user/{email}")]
        public async Task<IHttpActionResult> GetUserByEmail(string email)
        {
            var user = await this.AppUserManager.FindByEmailAsync(email);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        // POST api/Accounts/Register
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Email,
                Email = createUserModel.Email,
                JoinDate = DateTime.Now.Date,
                TwoFactorEnabled = true,
                PSK = OtpHelper.GenerateSharedPrivateKey(),
                IsActive = true
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user);
            IHttpActionResult errorResult = GetErrorResult(addUserResult);

            if (errorResult != null)
            {
                return errorResult;
            }

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new {userId = user.Id, code = code }));
            await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account",
                "Please confirm your account by click <a href=\"" + callbackUrl + "\">here</a>");

            return Ok();
        }

        // POST api/Accounts/CreateUser
        [Authorize(Roles = "Admin")]
        [Route("register")]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = createUserModel.Email,
                Email = createUserModel.Email,
                JoinDate = DateTime.Now.Date,
                TwoFactorEnabled = true,
                PSK = OtpHelper.GenerateSharedPrivateKey(),
                IsActive = true
            };

            IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user);
            IHttpActionResult errorResult = GetErrorResult(addUserResult);

            if (errorResult != null)
            {
                return errorResult;
            }

            string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
            await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account",
                "You have been registered for the Quest Tracker App. Please confirm your account by click <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }
            
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            
                IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

                if (result.Succeeded)
                {
                    return Ok(new { PSK = user.PSK }); // TODO setup account detail completion:  [FirstName],[LastName],[PhoneNumber],[PhoneNumberConfirmed]
                                                       // QRCode URL = HttpUtility.UrlEncode(KeyUrl.GetTotpUrl(Base32Encoder.Decode(user.PSK), user.Email) + "&issuer=QuestTracker")
            }
            else
                {
                    return GetErrorResult(result);
                }
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            // Only SuperAdmin or Admin can delete users 

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

        /// <summary>
        /// Sets a user inactive instead of deleting the user. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TwoFactorAuthorize]
        [Route("user/{id:guid}/setinactive")]
        public async Task<IHttpActionResult> SetUserInactive(string id)
        {
            // TODO only the user themselves can set inactive
            var appUser = await this.AppUserManager.FindByIdAsync(id);
            if (appUser != null)
            {

                appUser.IsActive = false;
                IdentityResult result = await this.AppUserManager.UpdateAsync(appUser);
                
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok();
            }
            return NotFound();

        }
 
        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExist = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExist.Any())
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' do not exist in the system", string.Join(",", rolesNotExist)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult =
                await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}