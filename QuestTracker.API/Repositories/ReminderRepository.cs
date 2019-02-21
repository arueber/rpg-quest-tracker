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
    public class ReminderRepository: RepositoryBase<Reminder>, IReminderRepository
    {
        public ReminderRepository(ApplicationContext applicationContext): base(applicationContext) { }


        public async Task<Reminder> GetReminderByItemIdAndUserIdAsync(int itemId, int userId)
        {
            var reminder =
                await FindByConditionAsync(r => r.ItemId.Equals(itemId) && r.ApplicationUserId.Equals(userId));
            return reminder.DefaultIfEmpty(new Reminder()).FirstOrDefault();
        }
        public async Task<Reminder> GetReminderByIdAsync(int reminderId)
        {
            var reminder = await FindByConditionAsync(r => r.Id.Equals(reminderId));
            return reminder.DefaultIfEmpty(new Reminder()).FirstOrDefault();
        }

        public async Task CreateReminderAsync(Reminder reminder)
        {
            Create(reminder);
            await SaveAsync();
        }
        public async Task UpdateReminderAsync(Reminder originalReminder, Reminder updatedReminder)
        {
            originalReminder.Map(updatedReminder);
            Update(originalReminder);
            await SaveAsync();
        }
        public async Task DeleteReminderAsync(Reminder reminder)
        {
            Delete(reminder);
            await SaveAsync();
        }
    }
}