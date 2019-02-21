using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Entities;
using QuestTracker.API.Models;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public interface IItemRepository: IRepositoryBase<Item>
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<IEnumerable<Item>> GetItemsByProjectIdAsync(int projectId, bool completedOnly);
        Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId, bool assignedOnly, bool priorityFlagOnly);
        Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId, DateTime dayToStart, TimeDelayType durationType, int durationCount, bool includeOverdue);
        Task<Item> GetItemByIdAsync(int itemId);

        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item originalItem, Item updatedItem);
        Task DeleteItemAsync(Item item);
    }
}
