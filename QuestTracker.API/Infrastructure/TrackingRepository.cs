using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using QuestTracker.API.Entities;
using QuestTracker.API.Models;

namespace QuestTracker.API.Infrastructure
{
    public class TrackingRepository: ITrackingRepository
    {
        private AuthContext _ctx;

        public TrackingRepository(AuthContext ctx)
        {
            _ctx = ctx;
        }

        #region Folders   
        public async Task<bool> Insert(Folder folder)
        {
            try
            {
                _ctx.Folders.Add(folder);
                return await _ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(Folder originalFolder, Folder updatedFolder)
        {
            _ctx.Entry(originalFolder).CurrentValues.SetValues(updatedFolder);

            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteFolder(int folderId)
        {
            try
            {
                var entity = await _ctx.Folders.FindAsync(folderId);
                if (entity != null)
                {
                    _ctx.Folders.Remove(entity);
                    return await _ctx.SaveChangesAsync() > 0;
                }
            }
            catch
            {
                // TODO: Logging
            }
            return false;
        }
        public async Task<Folder> GetFolder(int folderId)
        {
            return await _ctx.Folders.FindAsync(folderId);
        }
        public IQueryable<Folder> GetFoldersByUser(int userId)
        {
            return _ctx.Folders
                .Include("ProjectUsers")
                .Where(c => c.ProjectUsers.Any(s => s.User.Id == userId))
                .AsQueryable();
        }
        #endregion

        #region Projects
        public async Task<bool> Insert(Project project)
        {
            try
            {
                _ctx.Projects.Add(project);
                return await _ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(Project originalProject, Project updatedProject)
        {
            _ctx.Entry(originalProject).CurrentValues.SetValues(updatedProject);
            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteProject(int projectId)
        {
            try
            {
                var entity = await _ctx.Projects.FindAsync(projectId);
                if (entity != null)
                {
                    var projectusers = GetUsersByProject(projectId);
                    foreach (ProjectUser pu in projectusers)
                    {
                        if (pu.FolderId.HasValue)
                        {
                            Folder f = pu.Folder;
                            if (f.ProjectUsers.Count == 1)
                            {
                                bool resultF = await DeleteFolder(f.Id);
                                if (!resultF) throw new Exception("Delete Folder Failed. FolderId: " + f.Id);
                            }
                        }

                        bool resultPU = await DeleteProjectUser(pu.ProjectId, pu.ApplicationUserId);
                        if(!resultPU) throw new Exception("Delete ProjectUser Failed. ProjectId: " + pu.ProjectId + ", UserId: " + pu.ApplicationUserId);
                    }
                    
                    _ctx.Projects.Remove(entity);
                    return await _ctx.SaveChangesAsync() > 0;
                }
            }
            catch
            {
                // TODO: Logging
            }
            return false;
        }
        public async Task<Project> GetProject(int projectId)
        {
            return await _ctx.Projects.FindAsync(projectId);
        }
        #endregion


        #region ProjectUser
        public async Task<bool> Insert(ProjectUser projectUser)
        {
            try
            {
                _ctx.ProjectUsers.Add(projectUser);
                return await _ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(ProjectUser originalProjectUser, ProjectUser updatedProjectUser)
        {
            _ctx.Entry(originalProjectUser).CurrentValues.SetValues(updatedProjectUser);
            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteProjectUser(int projectId, int userId)
        {
            try
            {
                var entity = _ctx.ProjectUsers.FindAsync(projectId, userId);
                if (entity != null)
                {
                    _ctx.ProjectUsers.Remove(entity);
                    return await _ctx.SaveChangesAsync() > 0;
                }
            }
            catch
            {
                // TODO: Logging
            }
            return false;
        }
        public async Task<ProjectUser> GetProjectUser(int projectId, int userId)
        {
            return await _ctx.ProjectUsers.FindAsync(projectId, userId);
        }
        public IQueryable<ProjectUser> GetProjectsByFolder(int folderId)
        {
            return _ctx.ProjectUsers
                .Include("Project")
                .Include("User")
                .Include("Folder")
                .Where(c => c.Folder.Id == folderId)
                .AsQueryable();
        }
        public IQueryable<ProjectUser> GetProjectsByUser(int userId)
        {
            return _ctx.ProjectUsers
                .Include("Project")
                .Include("User")
                .Include("Folder")
                .Where(c => c.User.Id == userId)
                .AsQueryable();
        }
        public IQueryable<ProjectUser> GetUsersByProject(int projectId)
        {
            return _ctx.ProjectUsers
                .Include("Project")
                .Include("User")
                .Include("Folder")
                .Where(c => c.Project.Id == projectId)
                .AsQueryable();
        }
        #endregion

        #region Task Item
        public async Task<bool> Insert(Item item)
        {
            try
            {
                _ctx.Items.Add(item);
                return await _ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(Item originalItem, Item updatedItem)
        {
            _ctx.Entry(originalItem).CurrentValues.SetValues(updatedItem);
            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteItem(int itemId)
        {
            try
            {
                var entity = _ctx.Items.FindAsync(itemId);
                if (entity != null)
                {
                    _ctx.Items.Remove(entity);
                    return await _ctx.SaveChangesAsync() > 0;
                }
            }
            catch
            {
                // TODO: Logging
            }
            return false;
        }
        public async Task<Item> GetItem(int itemId)
        {
            return await _ctx.Items.FindAsync(itemId);
        }
        public IQueryable<Item> GetItemsByProject(int projectId, bool completedOnly)
        {
            return _ctx.Items
                .Include("Projects")
                .Where(c => c.Project.Id == projectId && (!completedOnly || c.CompletedAt != null))
                .AsQueryable();
        }
        public IQueryable<Item> GetItemsByUser(int userId, bool assignedOnly, bool priorityFlagOnly)
        {
            return _ctx.Items
                .Include("Projects")
                .Where(c => c.Project.ProjectUsers.Any(pu => pu.User.Id == userId) && c.CompletedAt == null && ((!assignedOnly || c.AssignedUser != null) && (!priorityFlagOnly || c.PriorityFlag)))
                .AsQueryable();
        }
        public IQueryable<Item> GetItemsByUser(int userId, DateTime dayToStart, TimeDelayType durationType, int durationCount, bool includeOverdue)
        {
            DateTime datetostop = dayToStart;
            switch (durationType)
            {
                    case TimeDelayType.Year:
                        datetostop = datetostop.AddYears(durationCount);
                        break;
                    case TimeDelayType.Month:
                        datetostop = datetostop.AddMonths(durationCount);
                        break;
                    case TimeDelayType.Week:
                        datetostop = datetostop.AddDays(durationCount * 7);
                        break;
                    case TimeDelayType.Day:
                        datetostop = datetostop.AddDays(durationCount);
                        break;
            }

            // TODO: figure out how to include Items with set DurationType and DurationCount - check if the event occurs during the date range or if the date range occurs during the event

            return _ctx.Items
                .Include("Projects")
                .Where(c => c.Project.ProjectUsers.Any(pu => pu.User.Id == userId) && c.CompletedAt == null 
                && (c.StartDueDate.Value.Date <= datetostop && (!includeOverdue || c.StartDueDate.Value.Date >= dayToStart)))
                .AsQueryable();
        }
        #endregion

        #region Reminders
        public async Task<bool> Insert(Reminder reminder)
        {
            try
            {
                _ctx.Reminders.Add(reminder);
                return await _ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(Reminder originalReminder, Reminder updatedReminder)
        {
            _ctx.Entry(originalReminder).CurrentValues.SetValues(updatedReminder);
            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteReminder(int reminderId)
        {
            try
            {
                var entity = _ctx.Reminders.FindAsync(reminderId);
                if (entity != null)
                {
                    _ctx.Reminders.Remove(entity);
                    return await _ctx.SaveChangesAsync() > 0;
                }
            }
            catch
            {
                // TODO: Logging
            }
            return false;
        }
        public async Task<Reminder> GetReminder(int reminderId)
        {
            return await _ctx.Reminders.FindAsync(reminderId);
        }
        public async Task<Reminder> GetReminderByItemAndUser(int itemId, int userId)
        {
            return await _ctx.Reminders.SingleOrDefaultAsync(c => c.Item.Id == itemId && c.ApplicationUser.Id == userId);
        }
        #endregion

        #region Sub Item
        public async Task<bool> Insert(SubItem subItem)
        {
            try
            {
                _ctx.SubItems.Add(subItem);
                return await _ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> Update(SubItem originalSubItem, SubItem updatedSubItem)
        {
            _ctx.Entry(originalSubItem).CurrentValues.SetValues(updatedSubItem);
            return await _ctx.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteSubItem(int subItemId)
        {
            try
            {
                var entity = _ctx.SubItems.FindAsync(subItemId);
                if (entity != null)
                {
                    _ctx.SubItems.Remove(entity);
                    return await _ctx.SaveChangesAsync() > 0;
                }
            }
            catch
            {
                // TODO: Logging
            }
            return false;
        }
        public async Task<SubItem> GetSubItem(int subItemId)
        {
            return await _ctx.SubItems.FindAsync(subItemId);
        }
        public IQueryable<SubItem> GetSubItemsByItem(int itemId)
        {
            return _ctx.SubItems.Where(c => c.ParentItem.Id == itemId).AsQueryable();
        }
        #endregion

        public async Task<bool> SaveAll()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}