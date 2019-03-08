using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var folders = this.AppContext.Folders.Select(f => new FolderDTO()
            {
                Id = f.Id,
                Title = f.Title,
                CreatedAt = f.CreatedAt.ToString("O"),
                CreatedByUserId = f.CreatedByUserId,
                UpdatedAt = f.UpdatedAt.ToString("O"),
                Revision = f.Revision,
                ProjectIds = f.ProjectUsers.Where(p => p.Accepted).Select(p => p.Project.Id).ToList()
            }).ToList();

            return Ok(folders);
        }
        
        // GET: api/Folders
        [HttpGet]
        public async Task<IHttpActionResult> GetFolders()
        {
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());

            if (user == null)
            {
                return NotFound();
            }

            var folders = user.Folders.Select(f => new FolderDTO()
            {
                Id = f.Id,
                Title = f.Title,
                CreatedAt = f.CreatedAt.ToString("O"),
                CreatedByUserId = f.CreatedByUserId,
                UpdatedAt = f.UpdatedAt.ToString("O"),
                Revision = f.Revision,
                ProjectIds = f.ProjectUsers.Where(p => p.Accepted).Select(p => p.Project.Id).ToList()
            }).ToList();

            return Ok(folders);
        }

        // GET: api/Folders/5
        [ResponseType(typeof(FolderDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> GetFolder(int id)
        {
            Folder folder = await this.AppContext.Folders.FindAsync(id);
            if (folder == null)
            {
                return NotFound();
            }
            var dto = new FolderDTO()
            {
                Id = folder.Id,
                Title = folder.Title,
                CreatedAt = folder.CreatedAt.ToString("O"),
                CreatedByUserId = folder.CreatedByUserId,
                UpdatedAt = folder.UpdatedAt.ToString("O"),
                Revision = folder.Revision,
                ProjectIds = folder.ProjectUsers.Where(p => p.Accepted).Select(p => p.Project.Id).ToList()
            };

            return Ok(dto);
        }

        // PUT: api/Folders/5
        [ResponseType(typeof(FolderDTO))]
        [HttpPut]
        public async Task<IHttpActionResult> PutFolder(int id, FolderPutOrDeleteBindingModel folder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Folder folderToPatch = await this.AppContext.Folders.FindAsync(id);
            if (folderToPatch == null)
            {
                return NotFound();
            }

            if (folderToPatch.Revision != folder.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            folderToPatch.Title = folder.Title;
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }
            List<ProjectUser> projects = user.ProjectUsers.Where(p => p.Accepted && folder.ProjectIds.Contains(p.ProjectId)).ToList();
            folderToPatch.ProjectUsers.Clear();
            foreach (ProjectUser pu in projects)
            {
                folderToPatch.ProjectUsers.Add(pu);
            }

            folderToPatch.Revision = folderToPatch.Revision + 1;
            folderToPatch.UpdatedAt = DateTime.UtcNow;
            this.AppContext.Entry(folderToPatch).State = EntityState.Modified;

            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var dto = new FolderDTO()
            {
                Id = folderToPatch.Id,
                Title = folderToPatch.Title,
                CreatedAt = folderToPatch.CreatedAt.ToString("O"),
                CreatedByUserId = folderToPatch.CreatedByUserId,
                UpdatedAt = folderToPatch.UpdatedAt.ToString("O"),
                Revision = folderToPatch.Revision,
                ProjectIds = folderToPatch.ProjectUsers.Where(p => p.Accepted).Select(p => p.Project.Id).ToList()
            };

            return Ok(dto);
        }

        // POST: api/Folders
        [ResponseType(typeof(FolderDTO))]
        [HttpPost]
        public async Task<IHttpActionResult> PostFolder([FromBody]FolderCreateBindingModel folder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }
            Folder createdFolder = new Folder(folder.Title, user.Id);
            try
            {
                this.AppContext.Folders.Add(createdFolder);
                List<ProjectUser> projects = user.ProjectUsers.Where(p => p.Accepted && folder.ProjectIds.Contains(p.ProjectId)).ToList();
                foreach (ProjectUser pu in projects)
                {
                    createdFolder.ProjectUsers.Add(pu);
                }
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var dto = new FolderDTO()
            {
                Id = createdFolder.Id,
                Title = createdFolder.Title,
                CreatedAt = createdFolder.CreatedAt.ToString("O"),
                CreatedByUserId = createdFolder.CreatedByUserId,
                UpdatedAt = createdFolder.UpdatedAt.ToString("O"),
                Revision = createdFolder.Revision,
                ProjectIds = createdFolder.ProjectUsers.Where(p => p.Accepted).Select(p => p.Project.Id).ToList()
            };

            return CreatedAtRoute("DefaultApi", new { id = createdFolder.Id }, dto);
        }

        // DELETE: api/Folders/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteFolder(int id, FolderPutOrDeleteBindingModel folder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Folder folderToDelete = await this.AppContext.Folders.FindAsync(id);
            if (folderToDelete == null)
            {
                return NotFound();
            }

            if (folderToDelete.Revision != folder.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            folderToDelete.IsActive = false;
            folderToDelete.UpdatedAt = DateTime.UtcNow;
            folderToDelete.Revision = folderToDelete.Revision + 1;
            this.AppContext.Entry(folderToDelete).State = EntityState.Modified;
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
    }
}
