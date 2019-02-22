using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.QueryObjects
{
    public class QueryObjects
    {

        //public async Task<IEnumerable<Folder>> GetFoldersByUserIdAsync(int userId)
        //{
        //    var items = await FindByConditionAsync(i => i.ProjectUsers.Any(s => s.User.Id == userId));
        //    return items;
        //}


        //public async Task<IEnumerable<Item>> GetItemsByProjectIdAsync(int projectId, bool completedOnly)
        //{
        //    var items = await FindByConditionAsync(i => i.ProjectId.Equals(projectId) && (!completedOnly || i.CompletedAt != null));
        //    return items;
        //}

        //public async Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId, bool assignedOnly, bool priorityFlagOnly)
        //{
        //    var items = await FindByConditionAsync(i => i.Project.ProjectUsers.Any(pu => pu.User.Id == userId) && i.CompletedAt == null && ((!assignedOnly || i.AssignedUser != null) && (!priorityFlagOnly || i.PriorityFlag)));
        //    return items;
        //}

        //public async Task<IEnumerable<Item>> GetItemsByUserIdAsync(int userId, DateTime dayToStart, TimeDelayType durationType, int durationCount, bool includeOverdue)
        //{
        //    DateTime datetostop = dayToStart;
        //    switch (durationType)
        //    {
        //        case TimeDelayType.Year:
        //            datetostop = datetostop.AddYears(durationCount);
        //            break;
        //        case TimeDelayType.Month:
        //            datetostop = datetostop.AddMonths(durationCount);
        //            break;
        //        case TimeDelayType.Week:
        //            datetostop = datetostop.AddDays(durationCount * 7);
        //            break;
        //        case TimeDelayType.Day:
        //            datetostop = datetostop.AddDays(durationCount);
        //            break;
        //    }

        //    // TODO: figure out how to include Items with set DurationType and DurationCount - check if the event occurs during the date range or if the date range occurs during the event

        //    var items = await FindByConditionAsync(c => c.Project.ProjectUsers.Any(pu => pu.User.Id == userId) && c.CompletedAt == null
        //                                                && (c.StartDueDate.Value.Date <= datetostop && (!includeOverdue || c.StartDueDate.Value.Date >= dayToStart)));
        //    return items;
        //}

        //public async Task<IEnumerable<ProjectUser>> GetProjectsByFolderIdAsync(int folderId)
        //{
        //    var projectUsers = await FindByConditionAsync(pu => pu.FolderId.Equals(folderId));
        //    return projectUsers;
        //}

        //public async Task<IEnumerable<ProjectUser>> GetProjectsByUserIdAsync(int userId)
        //{
        //    var projectUsers = await FindByConditionAsync(pu => pu.ApplicationUserId.Equals(userId));
        //    return projectUsers;
        //}

        //public async Task<IEnumerable<ProjectUser>> GetUsersByProjectIdAsync(int projectId)
        //{
        //    var projectUsers = await FindByConditionAsync(pu => pu.ProjectId.Equals(projectId));
        //    return projectUsers;
        //}

        //public async Task<ProjectUser> GetProjectUserByProjectIdAndUserIdAsync(int projectId, int userId)
        //{
        //    var projectUser =
        //        await FindByConditionAsync(pu => pu.ProjectId.Equals(projectId) && pu.ApplicationUserId.Equals(userId));
        //    return projectUser.DefaultIfEmpty(new ProjectUser()).FirstOrDefault();
        //}
        //public async Task<Reminder> GetReminderByItemIdAndUserIdAsync(int itemId, int userId)
        //{
        //    var reminder =
        //        await FindByConditionAsync(r => r.ItemId.Equals(itemId) && r.ApplicationUserId.Equals(userId));
        //    return reminder.DefaultIfEmpty(new Reminder()).FirstOrDefault();
        //}

        //public async Task<IEnumerable<SubItem>> GetSubItemsByItemIdAsync(int itemId)
        //{
        //    var subItems = await FindByConditionAsync(si => si.ParentItemId.Equals(itemId));
        //    return subItems;
        //}

        //public async Task<IEnumerable<TreeNode>> GetChildrenNodesByParentIdAsync(int parentNodeId)
        //{
        //    var nodes = await FindByConditionAsync(c => c.ParentNodeId.Equals(parentNodeId));
        //    return nodes;
        //}
        //public async Task<TreeNode> GetTreeNodeByItemIdAsync(int itemId)
        //{
        //    var node = await FindByConditionAsync(n => n.ItemId.Equals(itemId));
        //    return node.DefaultIfEmpty(new TreeNode()).FirstOrDefault();
        //}
        //public async Task<TreeNode> GetParentNodeByChildIdAsync(int childNodeId)
        //{
        //    var node = await FindByConditionAsync(p => p.ChildrenNodes.Any(c => c.Id.Equals(childNodeId)));
        //    return node.DefaultIfEmpty(new TreeNode()).FirstOrDefault();
        //}
        //public async Task<TreeNode> GetRootNodeByTreeNodeIdAsync(int nodeId)
        //{
        //    var nodes = await FindByConditionAsync(r => r.Id.Equals(nodeId));
        //    var node = nodes.DefaultIfEmpty(new TreeNode()).FirstOrDefault();
        //    while (node?.ParentNode != null)
        //    {
        //        node = node.ParentNode;
        //    }
        //    return node;
        //}
    }
}