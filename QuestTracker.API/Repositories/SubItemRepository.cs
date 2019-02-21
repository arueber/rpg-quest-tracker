using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using QuestTracker.API.Entities;
using QuestTracker.API.Entities.Extensions;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public class SubItemRepository:RepositoryBase<SubItem>, ISubItemRepository
    {
        public SubItemRepository(ApplicationContext applicationContext): base(applicationContext) { }

        public async Task<IEnumerable<SubItem>> GetSubItemsByItemIdAsync(int itemId)
        {
            var subItems = await FindByConditionAsync(si => si.ParentItemId.Equals(itemId));
            return subItems;
        }
        public async Task<SubItem> GetSubItemByIdAsync(int subItemId)
        {
            var subItem = await FindByConditionAsync(si => si.Id.Equals(subItemId));
            return subItem.DefaultIfEmpty(new SubItem()).FirstOrDefault();
        }

        public async Task CreateSubItemAsync(SubItem subItem)
        {
            Create(subItem);
            await SaveAsync();
        }
        public async Task UpdateSubItemAsync(SubItem originalSubItem, SubItem updatedSubItem)
        {
            originalSubItem.Map(updatedSubItem);
            Update(originalSubItem);
            await SaveAsync();
        }
        public async Task DeleteSubItemAsync(SubItem subItem)
        {
            Delete(subItem);
            await SaveAsync();
        }
    }
}