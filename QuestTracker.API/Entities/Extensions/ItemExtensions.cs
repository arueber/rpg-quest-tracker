using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class ItemExtensions
    {
        public static void Map(this Item dbItem, Item item)
        {
            dbItem.Name = item.Name;
            dbItem.Weight = item.Weight;
            dbItem.PriorityFlag = item.PriorityFlag;
            dbItem.URL = item.URL;
            dbItem.Notes = item.Notes;
            dbItem.StartDueDate = item.StartDueDate;
            dbItem.DurationType = item.DurationType;
            dbItem.DurationCount = item.DurationCount;
            dbItem.RepetitionType = item.RepetitionType;
            dbItem.RepetitionCount = item.RepetitionCount;
            dbItem.Revision = item.Revision;
            dbItem.CreatedAt = item.CreatedAt;
            dbItem.CompletedByUserId = item.CompletedByUserId;
            dbItem.AssignedUserId = item.AssignedUserId;
            dbItem.CompletedAt = item.CompletedAt;
            dbItem.CompletedByUserId = item.CompletedByUserId;
            dbItem.ProjectId = item.ProjectId;
        }
    }
}