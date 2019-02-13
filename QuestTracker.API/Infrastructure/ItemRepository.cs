using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using QuestTracker.API.Entities;

namespace QuestTracker.API.Infrastructure
{
    public class ItemRepository:IDisposable
    {
        private AuthContext _ctx;
        #region ctor
        public ItemRepository()
        {
            _ctx = new AuthContext();
        }
        #endregion

        //public Client FindClient(string clientId)
        //{
        //    var client = _ctx.Clients.Find(clientId);

        //    return client;
        //}
        #region Projects

        public List<Project> GetAllProjects()
        {
            return _ctx.Projects.ToList();
        }

        #endregion

        #region Items
        public async Task<bool> AddItem(Item item)
        {
            _ctx.Items.Add(item);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateItem(Item item)
        {
            var existingItem = _ctx.Items.SingleOrDefault(r => r.Id == item.Id);

            if (existingItem != null)
            {
                var result = await RemoveRefreshToken(existingItem);
            }

            _ctx.RefreshTokens.Add(token);

            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveItem(string itemId)
        {
            var item = await _ctx.Items.FindAsync(itemId);

            if (item != null)
            {
                _ctx.Items.Remove(item);
                return await _ctx.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveItem(Item item)
        {
            _ctx.Items.Remove(item);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<Item> FindItem(string itemId)
        {
            var item = await _ctx.Items.FindAsync(itemId);

            return item;
        }

        public List<Item> GetAllItems()
        {
            return _ctx.Items.ToList();
        }
        #endregion

        public void Dispose()
        {
            _ctx.Dispose();

        }
    }
}