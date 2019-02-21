using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class ProjectUserExtensions
    {
        public static void Map(this ProjectUser dbProjectUser, ProjectUser projectUser)
        {
            dbProjectUser.FolderId = projectUser.FolderId;
            dbProjectUser.Weight = projectUser.Weight;
            dbProjectUser.IsOwner = projectUser.IsOwner;
            dbProjectUser.DoNoDisturb = projectUser.DoNoDisturb;
            dbProjectUser.Accepted = projectUser.Accepted;
            dbProjectUser.Revision = projectUser.Revision;
        }
    }
}