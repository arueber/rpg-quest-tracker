using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class FolderExtensions
    {
        public static void Map(this Folder dbFolder, Folder folder)
        {
            dbFolder.CreatedByUserId = folder.CreatedByUserId;
            dbFolder.Title = folder.Title;
            dbFolder.IsActive = folder.IsActive;
            dbFolder.Weight = folder.Weight;
            dbFolder.CreatedAt = folder.CreatedAt;
            dbFolder.UpdatedAt = folder.UpdatedAt;
            dbFolder.Revision = folder.Revision;
        }
    }
}