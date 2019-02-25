using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using QuestTracker.API.Entities;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Projects")]
    public class ProjectsController : BaseApiController
    {

        // GET: api/Projects
        /// <summary>
        /// Get all Projects to which a user has permission.
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetProjects()
        {
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());

            if (user == null)
            {
                return NotFound();
            }
            var projects = user.ProjectUsers.Where(p => p.Accepted).OrderBy(p => p.Weight).Select(p => p.Project).ToList();

            return Ok(projects);
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await this.AppContext.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // PATCH: api/Projects/5
        [ResponseType(typeof(void))]
        [HttpPatch]
        public async Task<IHttpActionResult> PatchProject(int id, Project project)
        {

            // TODO Convert this to an actual Patch - how to impliment JsonPatch

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            this.AppContext.Entry(project).State = EntityState.Modified;

            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        [HttpPost]
        public async Task<IHttpActionResult> PostProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            this.AppContext.Projects.Add(project);
            await this.AppContext.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = project.Id }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            Project project = await this.AppContext.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            this.AppContext.Projects.Remove(project);
            await this.AppContext.SaveChangesAsync();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.AppContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return this.AppContext.Projects.Count(e => e.Id == id) > 0;
        }
    }
}