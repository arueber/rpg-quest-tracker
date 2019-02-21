using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using QuestTracker.API.Entities;
using QuestTracker.API.Entities.Extensions;
using QuestTracker.API.Models;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public class ItemRepository: RepositoryBase<Item>, IItemRepository
    {
        public ItemRepository(ApplicationContext applicationContext): base(applicationContext){}


        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            var items = await FindAllAsync();
            return items;
        }

        public async Task<IEnumerable<Item>> GetItemsByProjectIdAsync(int projectId, bool completedOnly)
        {
            var items = await FindByConditionAsync(i => i.ProjectId.Equals(projectId) && (!completedOnly || i.CompletedAt != null));
            return items;
        }

        public async Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId, bool assignedOnly, bool priorityFlagOnly)
        {
            var items = await FindByConditionAsync(i => i.Project.ProjectUsers.Any(pu => pu.User.Id == userId) && i.CompletedAt == null && ((!assignedOnly || i.AssignedUser != null) && (!priorityFlagOnly || i.PriorityFlag)));
            return items;
        }

        public async Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId, DateTime dayToStart, TimeDelayType durationType, int durationCount, bool includeOverdue)
        {
            DateTime datetostop = dayToStart;
            switch (durationType)
            {
                case TimeDelayType.Year:
                    datetostop = datetostop.AddYears(durationCount);
                    break;
                case TimeDelayType.Month:
                    datetostop = datetostop.AddMonths(durationCount);
                    break;
                case TimeDelayType.Week:
                    datetostop = datetostop.AddDays(durationCount * 7);
                    break;
                case TimeDelayType.Day:
                    datetostop = datetostop.AddDays(durationCount);
                    break;
            }

            // TODO: figure out how to include Items with set DurationType and DurationCount - check if the event occurs during the date range or if the date range occurs during the event

            var items = await FindByConditionAsync(c => c.Project.ProjectUsers.Any(pu => pu.User.Id == userId) && c.CompletedAt == null
                && (c.StartDueDate.Value.Date <= datetostop && (!includeOverdue || c.StartDueDate.Value.Date >= dayToStart)));
                return items;
        }

        public async Task<Item> GetItemByIdAsync(int itemId)
        {
            var item = await FindByConditionAsync(p => p.Id.Equals(itemId));
            return item.DefaultIfEmpty(new Item()).FirstOrDefault();
        }

        public async Task CreateItemAsync(Item item)
        {
            Create(item);
            await SaveAsync();
        }
        public async Task UpdateItemAsync(Item originalItem, Item updatedItem)
        {
            originalItem.Map(updatedItem);
            Update(originalItem);
            await SaveAsync();
        }
        public async Task DeleteItemAsync(Item item)
        {
            Delete(item);
            await SaveAsync();
        }
    }    
}