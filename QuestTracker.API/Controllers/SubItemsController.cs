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
using QuestTracker.API.Filters;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Models;

namespace QuestTracker.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/SubItems")]
    public class SubItemsController : BaseApiController
    {

        [Authorize(Users = "Admin")]
        [TwoFactorAuthorize]
        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetAllSubItems()
        {
            return Ok(this.AppContext.Items);
        }

        // GET api/SubItems
        [HttpGet]
        public async Task<IHttpActionResult> GetSubItems(int id, bool getForProject)
        {
            if (!getForProject)
            {
                Item taskItem = await this.AppContext.Items.FindAsync(id);
                if (taskItem == null)
                {
                    return NotFound();
                }

                var items = from i in taskItem.SubItems
                    select new SubItemDTO()
                    {
                        Id = i.Id,
                        Title = i.Title,
                        ItemId = i.ParentItemId,
                        CreatedAt = i.CreatedAt.ToString("O"),
                        CreatedById = i.CreatedByUserId,
                        IsCompleted = i.CompletedAt != null,
                        CompletedAt = i.CompletedAt?.ToString("O") ?? "",
                        Revision = i.Revision
                    };

                return Ok(items);
            }
            else
            {
                Project project = await this.AppContext.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                var items = from i in project.Items.SelectMany(i => i.SubItems)
                    select new SubItemDTO()
                    {
                        Id = i.Id,
                        Title = i.Title,
                        ItemId = i.ParentItemId,
                        CreatedAt = i.CreatedAt.ToString("O"),
                        CreatedById = i.CreatedByUserId,
                        IsCompleted = i.CompletedAt != null,
                        CompletedAt = i.CompletedAt?.ToString("O") ?? "",
                        Revision = i.Revision
                    };

                return Ok(items);
            }
        }

        // GET api/SubItems
        [HttpGet]
        public async Task<IHttpActionResult> GetSubItems(bool completed, int id, bool getForProject)
        {
            if (!getForProject)
            {
                Item taskItem = await this.AppContext.Items.FindAsync(id);
                if (taskItem == null)
                {
                    return NotFound();
                }

                var items = from i in taskItem.SubItems.Where(i => completed
                        ? i.CompletedAt != null
                        : i.CompletedAt == null)
                    select new SubItemDTO()
                    {
                        Id = i.Id,
                        Title = i.Title,
                        ItemId = i.ParentItemId,
                        CreatedAt = i.CreatedAt.ToString("O"),
                        CreatedById = i.CreatedByUserId,
                        IsCompleted = i.CompletedAt != null,
                        CompletedAt = i.CompletedAt?.ToString("O") ?? "",
                        Revision = i.Revision
                    };

                return Ok(items);
            }
            else
            {
                Project project = await this.AppContext.Projects.FindAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                var items = from i in project.Items.SelectMany(i => i.SubItems.Where(si => completed
                    ? si.CompletedAt != null
                    : si.CompletedAt == null))
                            select new SubItemDTO()
                    {
                        Id = i.Id,
                        Title = i.Title,
                        ItemId = i.ParentItemId,
                        CreatedAt = i.CreatedAt.ToString("O"),
                        CreatedById = i.CreatedByUserId,
                        IsCompleted = i.CompletedAt != null,
                        CompletedAt = i.CompletedAt?.ToString("O") ?? "",
                        Revision = i.Revision
                    };

                return Ok(items);
            }
            
        }

        // GET: api/SubItems/5
        [ResponseType(typeof(SubItemDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> GetSubItem(int id)
        {
            SubItem subItem = await this.AppContext.SubItems.FindAsync(id);
            if (subItem == null)
            {
                return NotFound();
            }
            var dto = new SubItemDTO()
            {
                Id = subItem.Id,
                Title = subItem.Title,
                ItemId = subItem.ParentItemId,
                CreatedAt = subItem.CreatedAt.ToString("O"),
                CreatedById = subItem.CreatedByUserId,
                IsCompleted = subItem.CompletedAt != null,
                CompletedAt = subItem.CompletedAt?.ToString("O") ?? "",
                Revision = subItem.Revision
            };

            return Ok(dto);
        }

        // PUT: api/SubItems/5
        [ResponseType(typeof(SubItemDTO))]
        [HttpPut]
        public async Task<IHttpActionResult> PutSubItem(int id, SubItemPutOrDeleteBindingModel subItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SubItem subItemToPatch = await this.AppContext.SubItems.FindAsync(id);
            if (subItemToPatch == null)
            {
                return NotFound();
            }

            if (subItemToPatch.Revision != subItem.Revision)
            {
                return BadRequest();
            }

            subItemToPatch.Title = subItem.Title;

            if (subItem.IsCompleted.HasValue && subItem.IsCompleted.Value && !subItemToPatch.CompletedAt.HasValue)
            {
                subItemToPatch.CompletedAt = DateTime.UtcNow;
            }
            else if ((!subItem.IsCompleted.HasValue || !subItem.IsCompleted.Value) && subItemToPatch.CompletedAt.HasValue)
            {
                subItemToPatch.CompletedAt = null;
            }
            subItemToPatch.Revision = subItemToPatch.Revision + 1;
            subItemToPatch.UpdatedAt = DateTime.UtcNow;

            this.AppContext.Entry(subItemToPatch).State = EntityState.Modified;

            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var dto = new SubItemDTO()
            {
                Id = subItemToPatch.Id,
                Title = subItemToPatch.Title,
                ItemId = subItemToPatch.ParentItemId,
                CreatedAt = subItemToPatch.CreatedAt.ToString("O"),
                CreatedById = subItemToPatch.CreatedByUserId,
                IsCompleted = subItemToPatch.CompletedAt.HasValue,
                CompletedAt = subItemToPatch.CompletedAt?.ToString("O") ?? "",
                Revision = subItemToPatch.Revision
            };

            return Ok(dto);
        }

        // POST: api/SubItems
        [ResponseType(typeof(SubItemDTO))]
        [HttpPost]
        public async Task<IHttpActionResult> PostSubItem([FromBody]SubItemCreateBindingModel subItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            SubItem createdSubItem = new SubItem(subItem.Title, subItem.ItemId, user.Id, subItem.Completed?? false);
            try
            {
                this.AppContext.SubItems.Add(createdSubItem);
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var dto = new SubItemDTO()
            {
                Id = createdSubItem.Id,
                Title = createdSubItem.Title,
                ItemId = createdSubItem.ParentItemId,
                CreatedAt = createdSubItem.CreatedAt.ToString("O"),
                CreatedById = createdSubItem.CreatedByUserId,
                IsCompleted = createdSubItem.CompletedAt != null,
                CompletedAt = createdSubItem.CompletedAt?.ToString("O") ?? "",
                Revision = createdSubItem.Revision
            };

            return CreatedAtRoute("DefaultApi", new { id = createdSubItem.Id }, dto);
        }

        // DELETE: api/SubItems/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteSubItem(int id, SubItemPutOrDeleteBindingModel subItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SubItem subItemToDelete = await this.AppContext.SubItems.FindAsync(id);
            if (subItemToDelete == null)
            {
                return NotFound();
            }

            if (subItemToDelete.Revision != subItem.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }
            subItemToDelete.IsActive = false;
            subItemToDelete.UpdatedAt = DateTime.UtcNow;
            subItemToDelete.Revision = subItemToDelete.Revision + 1;
            this.AppContext.Entry(subItemToDelete).State = EntityState.Modified;
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

        private bool SubItemExists(int id)
        {
            return this.AppContext.SubItems.Count(e => e.Id == id) > 0;
        }
    }
}