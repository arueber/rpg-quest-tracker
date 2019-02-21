using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Entities;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public interface ITreeNodeRepository:IRepositoryBase<TreeNode>
    {
        Task<IEnumerable<TreeNode>> GetChildrenNodesByParentIdAsync(int rootNodeId);
        Task<TreeNode> GetTreeNodeByIdAsync(int treeNodeId);
        Task<TreeNode> GetTreeNodeByItemIdAsync(int itemId);
        Task<TreeNode> GetParentNodeByChildIdAsync(int childNodeId);
        Task<TreeNode> GetRootNodeByTreeNodeIdAsync(int nodeId);

        Task CreateTreeNodeAsync(TreeNode treeNode);
        Task UpdateTreeNodeAsync(TreeNode originalNode, TreeNode updatedNode);
        Task DeleteTreeNodeAsync(TreeNode treeNode);
    }
}
