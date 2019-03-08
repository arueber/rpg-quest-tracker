using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using QuestTracker.API.Entities;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Models;

namespace QuestTracker.API.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class HomeController : BaseApiController
    {
        #region User(s)
        // GET: api/Users
        /// <summary>
        /// Get the users this user can access
        /// </summary>
        /// <param name="projectId">Optional, restricts the list of returned users to only those who have access to a particular project.</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        [Route("users")]
        public async Task<IHttpActionResult> GetUsers(int? projectId)
        {
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }

            // users from projectusers where projectid matches a list of projectids from the logged in user's projects
            var userprojectIds = user.ProjectUsers.Select(pu => pu.ProjectId).ToList();
            if (projectId != null)
            {
                if (userprojectIds.Contains((int) projectId))
                {
                    userprojectIds = new List<int> {(int) projectId};
                }
                else
                {
                    return NotFound();
                }
            }
            
            var users = from u in this.AppContext.ProjectUsers.Where(pu => userprojectIds.Contains(pu.ProjectId))
                    .Select(pu => pu.User).ToList()
                select new UserDTO()
                {
                    Id = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    JoinDate = u.JoinDate.ToString("O"),
                    Revision = u.Revision
                };

            return Ok(users);
        }

        // GET: api/User
        /// <summary>
        /// Get the currently logged in user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(UserDTO))]
        [Route("user")]
        public async Task<IHttpActionResult> GetUser()
        {
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }
            var dto = new UserDTO()
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                JoinDate = user.JoinDate.ToString("O"),
                Revision = user.Revision
            };

            return Ok(dto);
        }
        #endregion

        #region Memberships

        // GET: api/Users/5/Projects
        // GET: api/Projects/7/Users
        /// <summary>
        /// Get Memberships for a Project or the current User
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ProjectUserDTO))]
        [Route("memberships")]
        public async Task<IHttpActionResult> GetProjectUsers(int? projectId)
        {
            if (projectId == null)
            {
                ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user == null)
                {
                    return NotFound();
                }

                var projectusers = from p in user.ProjectUsers
                    select new ProjectUserDTO()
                    {
                        UserId = p.ApplicationUserId,
                        ProjectId = p.ProjectId,
                        State = p.Accepted?"accepted":"pending",
                        IsOwner = p.IsOwner,
                        DnD = p.DoNoDisturb,
                        Revision = p.Revision
                    };

                return Ok(projectusers);
            }
            else
            {
                var projectusers = from p in this.AppContext.ProjectUsers.Where(pu => pu.ProjectId == (int) projectId)
                    select new ProjectUserDTO()
                    {
                        UserId = p.ApplicationUserId,
                        ProjectId = p.ProjectId,
                        State = p.Accepted ? "accepted" : "pending",
                        IsOwner = p.IsOwner,
                        DnD = p.DoNoDisturb,
                        Revision = p.Revision
                    };
                return Ok(projectusers);
            }
        }

        // POST: api/memberships
        /// <summary>
        /// Add a User to a Project
        /// </summary>
        /// <param name="projectUser"></param>
        /// <returns></returns>
        [Route("memberships")]
        [HttpPost]
        [ResponseType(typeof(ProjectUserDTO))]
        public async Task<IHttpActionResult> PostProjectUser([FromBody]ProjectUserCreateBindingModel projectUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (projectUser.UserId == null && string.IsNullOrEmpty(projectUser.Email))
            {
                return BadRequest("Email or UserId is Required.");
            }
            Project project = await this.AppContext.Projects.FindAsync(projectUser.ProjectId);
            if (project == null)
            {
                return NotFound();
            }

            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }
            ProjectUser createdProjectUser = new ProjectUser();
            if (projectUser.UserId != null)
            {
                // TODO: check that logged in user has permissions to project (and is owner?) OR user is adding self to project?
                if (user.Id == projectUser.UserId)
                {
                    // logged in user is user to add to project => add user to project
                    createdProjectUser = new ProjectUser(project.Id, user.Id, true, projectUser.DnD ?? false);

                }
                else
                {
                    
                }



                // TODO: Check if user exists and send email to user if they don't already have an account (send email even if they do?)
                ApplicationUser userToAdd = await this.AppUserManager.FindByIdAsync((int)projectUser.UserId);
                if (userToAdd == null)
                {
                    if (string.IsNullOrEmpty(projectUser.Email))
                    {
                        return NotFound();
                    }

                }


            }
            
            // TODO: lots to do here

            


            // all roads lead to DTO
            var dto = new ProjectUserDTO()
            {
                UserId = createdProjectUser.ApplicationUserId,
                ProjectId = createdProjectUser.ProjectId,
                State = createdProjectUser.Accepted?"accepted":"pending",
                IsOwner = createdProjectUser.IsOwner,
                DnD = createdProjectUser.DoNoDisturb,
                Revision = createdProjectUser.Revision
            };

            return Ok(dto);
        }

        // PUT: api/Users/5/Projects/7
        // PUT: api/Projects/7/Users/5
        /// <summary>
        /// Mark a User as accepted for Project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <param name="projectUser"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("users/{userId}/projects/{projectId}")]
        [Route("projects/{projectId}/users/{userId}")]
        [ResponseType(typeof(ProjectUserDTO))]
        public async Task<IHttpActionResult> MarkUserAccepted([FromUri]int userId, [FromUri]int projectId, ProjectUserPutOrDeleteBindingModel projectUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProjectUser projectUserToDelete = await this.AppContext.ProjectUsers.FindAsync(projectId, userId);
            if (projectUserToDelete == null)
            {
                return NotFound();
            }
            if (projectUserToDelete.Revision != projectUser.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            // TODO: check that logged in user is same as userid
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }



            return Ok();
        }

        // DELETE: api/Users/5/Projects/7
        // DELETE: api/Projects/7/Users/5
        /// <summary>
        /// Remove a User from a Project OR Reject and invite to a Project
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <param name="projectUser"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("users/{userId}/projects/{projectId}")]
        [Route("projects/{projectId}/users/{userId}")]
        public async Task<IHttpActionResult> DeleteProjectUser([FromUri]int userId, [FromUri]int projectId, [FromBody]ProjectUserPutOrDeleteBindingModel projectUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectUser projectUserToDelete = await this.AppContext.ProjectUsers.FindAsync(projectId, userId);
            if (projectUserToDelete == null)
            {
                return NotFound();
            }

            if (projectUserToDelete.Revision != projectUser.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }

            // TODO: check if logged in user has access to the project and is owner OR check if logged in user is self-requesting project removal or invitation rejection

            //// users from projectusers where projectid matches a list of projectids from the logged in user's projects
            //var userprojectIds = user.ProjectUsers.Select(pu => pu.ProjectId).ToList();


            //var users = from u in this.AppContext.ProjectUsers.Where(pu => userprojectIds.Contains(pu.ProjectId))
            //        .Select(pu => pu.User).ToList()
            //    select new UserDTO()
            //    {
            //        Id = u.Id,
            //        FullName = $"{u.FirstName} {u.LastName}",
            //        Email = u.Email,
            //        JoinDate = u.JoinDate.ToString("O"),
            //        Revision = u.Revision
            //    };



            this.AppContext.ProjectUsers.Remove(projectUserToDelete);
            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        #endregion
    }
}
