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
using QuestTracker.API.Models;

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
        /// <returns>List of Projects</returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetProjects()
        {
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());

            if (user == null)
            {
                return NotFound();
            }
            var projects = user.ProjectUsers.Where(p => p.Accepted).OrderBy(p => p.Weight).Select(p => new ProjectDTO()
            {
                Id = p.Project.Id,
                CreatedAt = p.Project.CreatedAt.ToString("O"),
                Title = p.Project.Title,
                Revision = p.Project.Revision
            }).ToList();

            return Ok(projects);
        }

        // GET: api/Projects/5
        [ResponseType(typeof(ProjectDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await this.AppContext.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            var dto = new ProjectDTO()
            {
                Id = project.Id,
                CreatedAt = project.CreatedAt.ToString("O"),
                Title = project.Title,
                Revision = project.Revision
            };

            return Ok(dto);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(ProjectDTO))]
        [HttpPut]
        public async Task<IHttpActionResult> PutProject(int id, ProjectPutOrDeleteBindingModel project)
        {
            // 02/26/2019 | .Net Core includes Patch capabilities but there's no production-level packages to add Patch in Web Api 2 right now.

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Project projectToPatch = await this.AppContext.Projects.FindAsync(id);
            if (projectToPatch == null)
            {
                return NotFound();
            }
            
            if (projectToPatch.Revision != project.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            projectToPatch.Title = project.Title;
            projectToPatch.Revision = projectToPatch.Revision + 1;
            projectToPatch.UpdatedAt = DateTime.UtcNow;
            this.AppContext.Entry(projectToPatch).State = EntityState.Modified;

            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var dto = new ProjectDTO()
            {
                Id = projectToPatch.Id,
                Title = projectToPatch.Title,
                Revision = projectToPatch.Revision
            };

            return Ok(dto);
        }

        // POST: api/Projects
        [ResponseType(typeof(ProjectDTO))]
        [HttpPost]
        public async Task<IHttpActionResult> PostProject([FromBody]ProjectCreateBindingModel project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Project createdProject = new Project(project.Title);
            this.AppContext.Projects.Add(createdProject);
            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var dto = new ProjectDTO()
            {
                Id = createdProject.Id,
                CreatedAt = createdProject.CreatedAt.ToString("O"),
                Title = createdProject.Title,
                Revision = createdProject.Revision
            };

            return CreatedAtRoute("DefaultApi", new { id = createdProject.Id }, dto);
        }

        // DELETE: api/Projects/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteProject(int id, ProjectPutOrDeleteBindingModel project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            Project projectToDelete = await this.AppContext.Projects.FindAsync(id);
            if (projectToDelete == null)
            {
                return NotFound();
            }

            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null) return NotFound();

            ProjectUser projectUser = user.ProjectUsers.Where(pu => pu.ProjectId == projectToDelete.Id).DefaultIfEmpty(new ProjectUser()).FirstOrDefault();
            if (projectUser == null || projectUser.IsOwner != true)
            {
                return BadRequest("User does not have permissions to delete project.");
            }

            if (projectToDelete.Revision != project.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            projectToDelete.IsActive = false;
            projectToDelete.UpdatedAt = DateTime.UtcNow;
            projectToDelete.Revision = projectToDelete.Revision + 1;
            this.AppContext.Entry(projectToDelete).State = EntityState.Modified;
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