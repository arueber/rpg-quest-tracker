using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class SubItemExtensions
    {
        public static void Map(this SubItem dbSubItem, SubItem subItem)
        {
            dbSubItem.Name = subItem.Name;
            dbSubItem.CompletionDate = subItem.CompletionDate;
            dbSubItem.CreatedAt = subItem.CreatedAt;
            dbSubItem.CreatedByUserId = subItem.CreatedByUserId;
            dbSubItem.Revision = subItem.Revision;
            dbSubItem.ParentItemId = subItem.ParentItemId;
        }
    }
}