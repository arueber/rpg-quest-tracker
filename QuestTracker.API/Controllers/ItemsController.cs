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
    [RoutePrefix("api/Items")]
    public class ItemsController : BaseApiController
    {
        [Authorize(Users = "Admin")]
        [TwoFactorAuthorize]
        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetAllItems()
        {
            return Ok(this.AppContext.Items);
        }

        // GET api/Items
        [HttpGet]
        public async Task<IHttpActionResult> GetItems(int projectId)
        {
            Project project = await this.AppContext.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }
            
            var items = from i in project.Items
                        select new ItemDTO()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Weight = i.Weight,
                    PriorityFlag = i.PriorityFlag,
                    URL = i.URL,
                    Notes = i.Notes,
                    StartDueDate = i.StartDueDate?.ToString("O") ?? "",
                    DurationType = i.DurationType,
                    DurationCount = i.DurationCount,
                    RepetitionType = i.RepetitionType,
                    RepetitionCount = i.RepetitionCount,
                    RepetitionUsesRollingDate = i.RepetitionUsesRollingDate,
                    Revision = i.Revision,
                    AssignedId = i.AssignedUser.Id
                };
            return Ok(items);
        }

        // GET api/Items
        [HttpGet]
        public async Task<IHttpActionResult> GetItems(bool completed, int projectId)
        {
            Project project = await this.AppContext.Projects.FindAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var items = from i in project.Items.Where(i => completed
                ? (i.CompletedAt != null && i.CompletedByUserId != null)
                : (i.CompletedAt == null || i.CompletedByUserId == null)).ToList()
                select new ItemDTO()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Weight = i.Weight,
                    PriorityFlag = i.PriorityFlag,
                    URL = i.URL,
                    Notes = i.Notes,
                    StartDueDate = i.StartDueDate?.ToString("O") ?? "",
                    DurationType = i.DurationType,
                    DurationCount = i.DurationCount,
                    RepetitionType = i.RepetitionType,
                    RepetitionCount = i.RepetitionCount,
                    RepetitionUsesRollingDate = i.RepetitionUsesRollingDate,
                    Revision = i.Revision,
                    AssignedId = i.AssignedUser.Id
                };
            return Ok(items);
        }


        // GET: api/Items/5
        [ResponseType(typeof(ItemDTO))]
        public async Task<IHttpActionResult> GetItem(int id)
        {
            var item = await this.AppContext.Items.Include(i => i.AssignedUser).Select(i =>
                new ItemDTO()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Weight = i.Weight,
                    PriorityFlag = i.PriorityFlag,
                    URL = i.URL,
                    Notes = i.Notes,
                    StartDueDate = i.StartDueDate.HasValue ? i.StartDueDate.Value.ToString("O") : "",
                    DurationType = i.DurationType,
                    DurationCount = i.DurationCount,
                    RepetitionType = i.RepetitionType,
                    RepetitionCount = i.RepetitionCount,
                    RepetitionUsesRollingDate = i.RepetitionUsesRollingDate,
                    Revision = i.Revision,
                    AssignedId = i.AssignedUser.Id
                }).SingleOrDefaultAsync(i => i.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        // PUT: api/Items/5
        [ResponseType(typeof(ItemDTO))]
        [HttpPut]
        public async Task<IHttpActionResult> PutItem(int id, ItemPutOrDeleteBindingModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Item itemToPatch = await this.AppContext.Items.FindAsync(id);
            if (itemToPatch == null)
            {
                return NotFound();
            }

            if (itemToPatch.Revision != item.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());

            itemToPatch.Title = item.Title;
            itemToPatch.PriorityFlag = item.PriorityFlag;

            if (item.IsCompleted && !itemToPatch.CompletedAt.HasValue)
            {
                itemToPatch.CompletedAt = DateTime.UtcNow;
                itemToPatch.CompletedByUserId = user.Id;
            }else if (!item.IsCompleted && itemToPatch.CompletedAt.HasValue)
            {
                itemToPatch.CompletedAt = null;
                itemToPatch.CompletedByUserId = null;
            }

            if (item.ProjectId.HasValue) itemToPatch.ProjectId = item.ProjectId.Value;
            if (item.AssignedId.HasValue) itemToPatch.AssignedUserId = item.AssignedId.Value;
            if (string.IsNullOrEmpty(item.StartDueDate))
            {
                DateTime? startDueDate = TryParseNullable(item.StartDueDate);
                itemToPatch.StartDueDate = startDueDate;
            }
            
            if (TimeFrameIsValid(item.DurationType, item.DurationCount))
            {
                itemToPatch.DurationType = GetTimeFrameType(item.DurationType);
                itemToPatch.DurationCount = item.DurationCount;
            }
            
            if (TimeFrameIsValid(item.RepetitionType, item.RepetitionCount))
            {
                itemToPatch.RepetitionType = GetTimeFrameType(item.RepetitionType);
                itemToPatch.RepetitionCount = item.RepetitionCount;
                itemToPatch.RepetitionUsesRollingDate = item.RepetitionUsesRollingDate;
            }

            foreach (string removeAttribute in item.RemoveAttributes)
            {
                switch (removeAttribute)
                {
                    case "AssignedId":
                        itemToPatch.AssignedUserId = null;
                        continue;
                    case "StartDueDate":
                        itemToPatch.StartDueDate = null;
                        continue;
                    case "Duration":
                        itemToPatch.DurationType = null;
                        itemToPatch.DurationCount = null;
                        continue;
                    case "Repetition":
                        itemToPatch.RepetitionType = null;
                        itemToPatch.RepetitionCount = null;
                        itemToPatch.RepetitionUsesRollingDate = false;
                        continue;
                    default:
                        continue;
                }
            }

            this.AppContext.Entry(itemToPatch).State = EntityState.Modified;

            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var dto = new ItemDTO()
            {
                Id = itemToPatch.Id,
                Title = itemToPatch.Title,
                ProjectId = itemToPatch.ProjectId,
                Weight = itemToPatch.Weight,
                PriorityFlag = itemToPatch.PriorityFlag,
                URL = itemToPatch.URL,
                Notes = itemToPatch.Notes,
                StartDueDate = itemToPatch.StartDueDate?.ToString("O"),
                DurationType = itemToPatch.DurationType,
                DurationCount = itemToPatch.DurationCount,
                RepetitionType = itemToPatch.RepetitionType,
                RepetitionCount = itemToPatch.RepetitionCount,
                AssignedId = itemToPatch.AssignedUser.Id,
                Revision = itemToPatch.Revision
            };

            return Ok(dto);
        }

        // POST: api/Items
        [HttpPost]
        [ResponseType(typeof(ItemDTO))]
        public async Task<IHttpActionResult> PostItem([FromBody]ItemCreateBindingModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());

            int? completedId = null;
            if (item.IsCompleted ?? false)
            {
                completedId = user.Id;}

            DateTime? startDueDate = TryParseNullable(item.StartDueDate);

            TimeDelayType? durationType = null;
            int? durationCount = null;
            if(TimeFrameIsValid(item.DurationType, item.DurationCount)) {
                durationType = GetTimeFrameType(item.DurationType);
                durationCount = item.DurationCount;
            }
            TimeDelayType? repetitionType = null;
            int? repetitionCount = null;
            bool repetitionIsRolling = false;
            if (TimeFrameIsValid(item.RepetitionType, item.RepetitionCount))
            {
                repetitionType = GetTimeFrameType(item.RepetitionType);
                repetitionCount = item.RepetitionCount;
                if (item.RepetitionUsesRollingDate.HasValue)
                    repetitionIsRolling = item.RepetitionUsesRollingDate ?? false;
            }

            Item createdItem = new Item(item.Title, item.ProjectId, user.Id, item.IsCompleted ?? false, item.PriorityFlag ?? false, 
                item.AssignedId, completedId, startDueDate, durationType, durationCount, repetitionType, repetitionCount, repetitionIsRolling);

            this.AppContext.Items.Add(createdItem);
            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
               Console.WriteLine(e);
                throw;
            }

            this.AppContext.Entry(createdItem).Reference(x => x.AssignedUser).Load();

            var dto = new ItemDTO()
            {
                Id = createdItem.Id,
                Title = createdItem.Title,
                ProjectId = createdItem.ProjectId,
                Weight = createdItem.Weight,
                PriorityFlag = createdItem.PriorityFlag,
                URL = createdItem.URL,
                Notes = createdItem.Notes,
                StartDueDate = createdItem.StartDueDate?.ToString("O"),
                DurationType = createdItem.DurationType,
                DurationCount = createdItem.DurationCount,
                RepetitionType = createdItem.RepetitionType,
                RepetitionCount = createdItem.RepetitionCount,
                AssignedId = createdItem.AssignedUser.Id,
                Revision = createdItem.Revision
            };

            return CreatedAtRoute("DefaultApi", new { id = createdItem.Id }, dto);
        }

        // DELETE: api/Items/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteItem(int id, ItemPutOrDeleteBindingModel item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            Item itemToDelete = await this.AppContext.Items.FindAsync(id);
            if (itemToDelete == null)
            {
                return NotFound();
            }

            if (itemToDelete.Revision != item.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            itemToDelete.IsActive = false;
            itemToDelete.UpdatedAt = DateTime.UtcNow;
            itemToDelete.Revision = itemToDelete.Revision + 1;
            this.AppContext.Entry(itemToDelete).State = EntityState.Modified;

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

        private bool ItemExists(int id)
        {
            return this.AppContext.Items.Count(e => e.Id == id) > 0;
        }

        private bool TimeFrameIsValid(string type, int? count)
        {
            return (type != "" && count != null);
        }

        private TimeDelayType? GetTimeFrameType(string type)
        {
            switch (type)
            {
                case "day":
                    return TimeDelayType.Day;
                case "week":
                    return TimeDelayType.Week;
                case "month":
                    return TimeDelayType.Month;
                case "year":
                    return TimeDelayType.Year;
                default:
                    return null;
            }
        }

        private static DateTime? TryParseNullable(string val)
        {
            DateTime outValue;
            return DateTime.TryParse(val, out outValue) ? (DateTime?) outValue : null;
        }
    }
}