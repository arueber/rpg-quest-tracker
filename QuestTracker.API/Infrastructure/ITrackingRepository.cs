using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;
using QuestTracker.API.Entities;
using QuestTracker.API.Models;

namespace QuestTracker.API.Infrastructure
{
    public interface ITrackingRepository
    {
        #region Folders   
        bool Insert(Folder folder);
        bool Update(Folder originalFolder, Folder updatedFolder);
        bool DeleteFolder(int id);
        Folder GetFolder(int folderId);
        IQueryable<Folder> GetFoldersByUser(int userId);
        #endregion

        #region Projects
        bool Insert(Project project);
        bool Update(Project originalProject, Project updatedProject);
        bool DeleteProject(int id, bool deleteFolder);
        Project GetProject(int projectId);
        #endregion


        #region ProjectUser
        bool Insert(ProjectUser projectUser);
        bool Update(ProjectUser originalProjectUser, ProjectUser updatedProjectUser);
        bool DeleteProjectUser(int projectId, int userId);
        IQueryable<ProjectUser> GetProjectsByFolder(int folderId);
        IQueryable<ProjectUser> GetProjectsByUser(int userId);
        IQueryable<ProjectUser> GetUsersByProject(int projectId);
        #endregion

        #region Task Item
        bool Insert(Item item);
        bool Update(Item originalItem, Item updatedItem);
        bool DeleteItem(int id);
        Item GetItem(int itemId);
        IQueryable<Item> GetItemsByProject(int projectId, bool completedOnly);
        IQueryable<Item> GetItemsByUser(int userId, bool assignedOnly, bool priorityFlagOnly);
        IQueryable<Item> GetItemsByUser(int userId, DateTime today, TimeDelayType durationType, int durationCount, bool includeOverdue);
        #endregion

        #region Reminders
        bool Insert(Reminder reminder);
        bool Update(Reminder originalReminder, Reminder updatedReminder);
        bool DeleteReminder(int id);
        Reminder GetReminder(int id);
        Reminder GetReminderByItemAndUser(int itemId, int userId);
        #endregion

        #region Sub Item
        bool Insert(SubItem item);
        bool Update(SubItem originalSubItem, SubItem updatedSubItem);
        bool DeleteSubItem(int id);
        SubItem GetSubItem(int subItemId);
        IQueryable<SubItem> GetSubItemsByItem(int itemId);
        #endregion

        bool SaveAll();
    }
}
