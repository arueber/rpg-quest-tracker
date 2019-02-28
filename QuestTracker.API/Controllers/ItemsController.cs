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
        private ApplicationContext db = new ApplicationContext();

        [Authorize(Users = "Admin")]
        [TwoFactorAuthorize]
        [Route("All")]
        public IHttpActionResult GetAllItems()
        {
            return Ok(db.Items);
        }

        // GET api/Items
        [Route("")]
        public IQueryable<ItemDTO> GetItems()
        {
            var items = from i in db.Items
                select new ItemDTO()
                {
                    Id = i.Id,
                    Title = i.Title,
                    Weight = i.Weight,
                    PriorityFlag = i.PriorityFlag,
                    URL = i.URL,
                    Notes = i.Notes,
                    StartDueDate = i.StartDueDate.HasValue? i.StartDueDate.Value.ToString("O"):"",
                    DurationType = i.DurationType,
                    DurationCount = i.DurationCount,
                    RepetitionType = i.RepetitionType,
                    RepetitionCount = i.RepetitionCount,
                    Revision = i.Revision,
                    AssignedId = i.AssignedUser.Id
                };
            return items;
        }

        // GET: api/Items/5
        [ResponseType(typeof(ItemDTO))]
        public async Task<IHttpActionResult> GetItem(int id)
        {
            var item = await db.Items.Include(i => i.AssignedUser).Select(i =>
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
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
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

        // POST: api/Items
        [HttpPost]
        [ResponseType(typeof(ItemDTO))]
        public async Task<IHttpActionResult> PostItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ItemExists(item.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            db.Entry(item).Reference(x => x.AssignedUser).Load();

            var dto = new ItemDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Weight = item.Weight,
                PriorityFlag = item.PriorityFlag,
                URL = item.URL,
                Notes = item.Notes,
                StartDueDate = item.StartDueDate?.ToString("O"),
                DurationType = item.DurationType,
                DurationCount = item.DurationCount,
                RepetitionType = item.RepetitionType,
                RepetitionCount = item.RepetitionCount,
                Revision = item.Revision,
                AssignedId = item.AssignedUser.Id
            };

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, dto);
        }

        // DELETE: api/Items/5
        [ResponseType(typeof(ItemDTO))]
        public async Task<IHttpActionResult> DeleteItem(string id)
        {
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            var dto = new ItemDTO()
            {
                Id = item.Id,
                Title = item.Title,
                Weight = item.Weight,
                PriorityFlag = item.PriorityFlag,
                URL = item.URL,
                Notes = item.Notes,
                StartDueDate = item.StartDueDate?.ToString("O") ?? "",
                DurationType = item.DurationType,
                DurationCount = item.DurationCount,
                RepetitionType = item.RepetitionType,
                RepetitionCount = item.RepetitionCount,
                Revision = item.Revision,
                AssignedId = item.AssignedUser.Id
            };

            return Ok(dto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }

        private bool RecurrenceIsValid(string type, int? count)
        {
            return (type != "" && count != null);
        }

        private TimeDelayType? GetRecurrenceType(string type)
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
    }
}