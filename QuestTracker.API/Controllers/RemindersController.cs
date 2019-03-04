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
    [RoutePrefix("api/Reminders")]
    public class RemindersController : BaseApiController
    {
        // GET: api/Reminders
        public IQueryable<Reminder> GetReminders()
        {
            return db.Reminders;
        }

        // GET: api/Reminders/5
        [ResponseType(typeof(Reminder))]
        public async Task<IHttpActionResult> GetReminder(int id)
        {
            Reminder reminder = await db.Reminders.FindAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            return Ok(reminder);
        }

        // PUT: api/Reminders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReminder(int id, Reminder reminder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reminder.Id)
            {
                return BadRequest();
            }

            db.Entry(reminder).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReminderExists(id))
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

            
            Reminder createdReminder = new Reminder(reminder.ItemId, (DateTime) date, user.Id, reminder.CreatedByDeviceUDID);

            db.Reminders.Add(reminder);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = reminder.Id }, reminder);
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