using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Entities;

namespace QuestTracker.API.Infrastructure
{
    public interface ITreeRepository
    {
        #region Reminders
        bool Insert(TreeNode treeNode);
        bool Update(TreeNode originalNode, TreeNode updatedNode);
        bool DeleteTreeNode(int id);
        TreeNode GetTreeNode(int id);
        TreeNode GetTreeNodeByItem(int itemId);
        TreeNode GetParentNode(int childNodeId);
        TreeNode GetRootNodeByTreeNode(int nodeId);
        IQueryable<TreeNode> GetChildrenNodesByParent(int rootNodeId);
        #endregion

        bool SaveAll();
    }
}
