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
    [RoutePrefix("api/Reminders")]
    public class RemindersController : BaseApiController
    {
        [Authorize(Users = "Admin")]
        [TwoFactorAuthorize]
        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetAllReminders()
        {
            return Ok(this.AppContext.Reminders);
        }

        // GET: api/Reminders
        [HttpGet]
        public async Task<IHttpActionResult> GetReminders(int itemId)
        {
            Item taskItem = await this.AppContext.Items.FindAsync(itemId);
            if (taskItem == null)
            {
                return NotFound();
            }

            var reminders = from r in taskItem.Reminders
                select new ReminderDTO()
                {
                    Id = r.Id,
                    Date = r.Date.ToString("O"),
                    ItemId = r.ItemId,
                    CreatedAt = r.CreatedAt.ToString("O"),
                    UpdatedAt = r.UpdatedAt.ToString("O"),
                    Revision = r.Revision
                };
            return Ok(reminders);
        }

        // GET: api/Reminders/5
        [ResponseType(typeof(ReminderDTO))]
        [HttpGet]
        public async Task<IHttpActionResult> GetReminder(int id)
        {
            Reminder reminder = await this.AppContext.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            var dto = new ReminderDTO()
            {
                Id = reminder.Id,
                Date = reminder.Date.ToString("O"),
                ItemId = reminder.ItemId,
                CreatedAt = reminder.CreatedAt.ToString("O"),
                UpdatedAt = reminder.UpdatedAt.ToString("O"),
                Revision = reminder.Revision
            };

            return Ok(dto);
        }

        // PUT: api/Reminders/5
        [ResponseType(typeof(ReminderDTO))]
        [HttpPut]
        public async Task<IHttpActionResult> PutReminder(int id, ReminderPutOrDeleteBindingModel reminder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Reminder reminderToPatch = await this.AppContext.Reminders.FindAsync(id);
            if (reminderToPatch == null)
            {
                return NotFound();
            }

            if (reminderToPatch.Revision != reminder.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            DateTime? date = TryParseNullable(reminder.Date);
            if (date == null)
            {
                return BadRequest("Date field must be a valid date.");
            }
            reminderToPatch.Date = (DateTime) date;
            reminderToPatch.Revision = reminderToPatch.Revision + 1;
            reminderToPatch.UpdatedAt = DateTime.UtcNow;
            this.AppContext.Entry(reminderToPatch).State = EntityState.Modified;

            try
            {
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var dto = new ReminderDTO()
            {
                Id = reminderToPatch.Id,
                Date = reminderToPatch.Date.ToString("O"),
                ItemId = reminderToPatch.ItemId,
                CreatedAt = reminderToPatch.CreatedAt.ToString("O"),
                UpdatedAt = reminderToPatch.UpdatedAt.ToString("O"),
                Revision = reminderToPatch.Revision
            };

            return Ok(dto);
        }

        // POST: api/Reminders
        [ResponseType(typeof(Reminder))]
        public async Task<IHttpActionResult> PostReminder([FromBody]ReminderCreateBindingModel reminder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DateTime? date = TryParseNullable(reminder.Date);
            if (date == null)
            {
                return BadRequest("Date field must be a valid date.");
            }

            ApplicationUser user = await this.AppUserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return NotFound();
            }

            Reminder createdReminder = new Reminder(reminder.ItemId, (DateTime) date, user.Id, reminder.CreatedByDeviceUDID);
            try
            {
                this.AppContext.Reminders.Add(createdReminder);
                await this.AppContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var dto = new ReminderDTO()
            {
                Id = createdReminder.Id,
                Date = createdReminder.Date.ToString("O"),
                ItemId = createdReminder.ItemId,
                CreatedAt = createdReminder.CreatedAt.ToString("O"),
                UpdatedAt = createdReminder.UpdatedAt.ToString("O"),
                Revision = createdReminder.Revision
            };

            return CreatedAtRoute("DefaultApi", new { id = dto.Id }, dto);
        }

        // DELETE: api/Reminders/5
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteReminder(int id, ReminderPutOrDeleteBindingModel reminder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Reminder reminderToDelete = await this.AppContext.Reminders.FindAsync(id);
            if (reminderToDelete == null)
            {
                return NotFound();
            }

            if (reminderToDelete.Revision != reminder.Revision)
            {
                return BadRequest("Revision does not match. Fetch the entity's current state and try again");
            }

            try
            {
                this.AppContext.Reminders.Remove(reminderToDelete);
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

        private bool ReminderExists(int id)
        {
            return this.AppContext.Reminders.Count(e => e.Id == id) > 0;
        }
        private static DateTime? TryParseNullable(string val)
        {
            DateTime outValue;
            return DateTime.TryParse(val, out outValue) ? (DateTime?)outValue : null;
        }
    }
}