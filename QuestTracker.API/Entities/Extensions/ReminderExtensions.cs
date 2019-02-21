using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class ReminderExtensions
    {
        public static void Map(this Reminder dbReminder, Reminder reminder)
        {
            dbReminder.Date = reminder.Date;
            dbReminder.Revision = reminder.Revision;
            dbReminder.CreatedAt = reminder.CreatedAt;
            dbReminder.CreatedByDeviceUDID = reminder.CreatedByDeviceUDID;
            dbReminder.UpdatedAt = reminder.UpdatedAt;
            dbReminder.ApplicationUserId = reminder.ApplicationUserId;
            dbReminder.ItemId = reminder.ItemId;
        }
    }
}