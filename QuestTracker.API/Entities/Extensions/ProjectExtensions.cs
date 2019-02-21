using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class ProjectExtensions
    {
        public static void Map(this Project dbProject, Project project)
        {
            dbProject.Name = project.Name;
            dbProject.IsActive = project.IsActive;
            dbProject.CreatedAt = project.CreatedAt;
            dbProject.Revision = project.Revision;
        }
    }
}