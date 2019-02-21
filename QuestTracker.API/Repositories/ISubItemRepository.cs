using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Entities;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public interface ISubItemRepository: IRepositoryBase<SubItem>
    {
        Task<IEnumerable<SubItem>> GetSubItemsByItemIdAsync(int itemId);
        Task<SubItem> GetSubItemByIdAsync(int subItemId);

        Task CreateSubItemAsync(SubItem subItem);
        Task UpdateSubItemAsync(SubItem originalSubItem, SubItem updatedSubItem);
        Task DeleteSubItemAsync(SubItem subItem);
    }
}
