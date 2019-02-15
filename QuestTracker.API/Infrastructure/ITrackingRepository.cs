using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using QuestTracker.API.Entities;

namespace QuestTracker.API.Infrastructure
{
    public interface ITrackingRepository
    {
        IQueryable<Folder> GetFoldersByUser(int userId);
        Folder GetFolder(int folderId);

        IQueryable<ProjectUser> GetProjectsByFolder(int folderId);
        IQueryable<ProjectUser> GetProjectsByUser(int userId);
        Folder GetFolderForProject(int projectId);

        IQueryable<Item> GetItemsByProject(int projectId);
       // IQueryable<Item> GetItemsByUserAndDate(int userId, DateTime today, TimeSpan duration = TimeSpan.TicksPerDay.ToString()); TODO Doesn't work...
        IQueryable<Item> GetItemsByUserAndPriorityFlag(int userId);
        IQueryable<Item> GetItemsByAssignedUser(int userId);
        Item GetItem(int itemId);

        IQueryable<SubItem> GetSubItemsByItem(int itemId);
        SubItem GetSubItem(int subItemId);

        bool Insert(Folder folder);
        bool Update(Folder originalFolder, Folder updatedFolder);
        bool DeleteFolder(int id);

        bool Insert(Project project);
        bool Update(Project originalProject, Project updatedProject);
        bool DeleteProject(int id);

        bool Insert(ProjectUser projectUser);
        bool Update(ProjectUser originalProjectUser, ProjectUser updatedProjectUser);
        bool DeleteProjectUser(int projectId, int userId);

        bool Insert(Item item);
        bool Update(Item originalItem, Item updatedItem);
        bool DeleteItem(int id);

        bool Insert(Reminder reminder);
        bool Update(Reminder originalReminder, Reminder updatedReminder);
        bool DeleteReminder(int id);

        bool Insert(SubItem item);
        bool Update(SubItem originalSubItem, SubItem updatedSubItem);
        bool DeleteSubItem(int id);

        bool SaveAll();
    }
}
