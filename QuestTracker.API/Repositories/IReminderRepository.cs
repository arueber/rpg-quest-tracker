using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Entities;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public interface IReminderRepository: IRepositoryBase<Reminder>
    {
        Task<Reminder> GetReminderByItemIdAndUserIdAsync(int itemId, int userId);
        Task<Reminder> GetReminderByIdAsync(int reminderId);

        Task CreateReminderAsync(Reminder reminder);
        Task UpdateReminderAsync(Reminder originalReminder, Reminder updatedReminder);
        Task DeleteReminderAsync(Reminder reminder);
    }
}
