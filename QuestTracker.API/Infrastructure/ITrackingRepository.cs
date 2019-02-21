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
        Task<bool> Insert(Folder folder);
        Task<bool> Update(Folder originalFolder, Folder updatedFolder);
        Task<bool> DeleteFolder(int folderId);
        Task<Folder> GetFolder(int folderId);
        IQueryable<Folder> GetFoldersByUser(int userId);
        #endregion

        #region Projects
        Task<bool> Insert(Project project);
        Task<bool> Update(Project originalProject, Project updatedProject);
        Task<bool> DeleteProject(int projectId);
        Task<Project> GetProject(int projectId);
        #endregion


        #region ProjectUser
        Task<bool> Insert(ProjectUser projectUser);
        Task<bool> Update(ProjectUser originalProjectUser, ProjectUser updatedProjectUser);
        Task<bool> DeleteProjectUser(int projectId, int userId);
        Task<ProjectUser> GetProjectUser(int projectId, int userId);
        IQueryable<ProjectUser> GetProjectsByFolder(int folderId);
        IQueryable<ProjectUser> GetProjectsByUser(int userId);
        IQueryable<ProjectUser> GetUsersByProject(int projectId);
        #endregion

        #region Task Item
        Task<bool> Insert(Item item);
        Task<bool> Update(Item originalItem, Item updatedItem);
        Task<bool> DeleteItem(int itemId);
        Task<Item> GetItem(int itemId);
        IQueryable<Item> GetItemsByProject(int projectId, bool completedOnly);
        IQueryable<Item> GetItemsByUser(int userId, bool assignedOnly, bool priorityFlagOnly);
        IQueryable<Item> GetItemsByUser(int userId, DateTime today, TimeDelayType durationType, int durationCount, bool includeOverdue);
        #endregion

        #region Reminders
        Task<bool> Insert(Reminder reminder);
        Task<bool> Update(Reminder originalReminder, Reminder updatedReminder);
        Task<bool> DeleteReminder(int reminderId);
        Task<Reminder> GetReminder(int reminderId);
        Task<Reminder> GetReminderByItemAndUser(int itemId, int userId);
        #endregion

        #region Sub Item
        Task<bool> Insert(SubItem subItem);
        Task<bool> Update(SubItem originalSubItem, SubItem updatedSubItem);
        Task<bool> DeleteSubItem(int subItemId);
        Task<SubItem> GetSubItem(int subItemId);
        IQueryable<SubItem> GetSubItemsByItem(int itemId);
        #endregion

        Task<bool> SaveAll();
    }
}
