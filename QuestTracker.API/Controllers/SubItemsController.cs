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
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Controllers
{
    public class SubItemsController : ApiController
    {
        private ApplicationContext db = new ApplicationContext();

        // GET: api/SubItems
        public IQueryable<SubItem> GetSubItems()
        {
            return db.SubItems;
        }

        // GET: api/SubItems/5
        [ResponseType(typeof(SubItem))]
        public async Task<IHttpActionResult> GetSubItem(int id)
        {
            SubItem subItem = await db.SubItems.FindAsync(id);
            if (subItem == null)
            {
                return NotFound();
            }

            return Ok(subItem);
        }

        // PUT: api/SubItems/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSubItem(int id, SubItem subItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subItem.Id)
            {
                return BadRequest();
            }

            db.Entry(subItem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubItemExists(id))
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

        // POST: api/SubItems
        [ResponseType(typeof(SubItem))]
        public async Task<IHttpActionResult> PostSubItem(SubItem subItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SubItems.Add(subItem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = subItem.Id }, subItem);
        }

        // DELETE: api/SubItems/5
        [ResponseType(typeof(SubItem))]
        public async Task<IHttpActionResult> DeleteSubItem(int id)
        {
            SubItem subItem = await db.SubItems.FindAsync(id);
            if (subItem == null)
            {
                return NotFound();
            }

            db.SubItems.Remove(subItem);
            await db.SaveChangesAsync();

            return Ok(subItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubItemExists(int id)
        {
            return db.SubItems.Count(e => e.Id == id) > 0;
        }
    }
}