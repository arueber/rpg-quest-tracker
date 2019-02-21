using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using QuestTracker.API.Entities;
using QuestTracker.API.Entities.Extensions;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public class TreeNodeRepository: RepositoryBase<TreeNode>, ITreeNodeRepository
    {
        public TreeNodeRepository(ApplicationContext applicationContext): base(applicationContext) { }

        public async Task<IEnumerable<TreeNode>> GetChildrenNodesByParentIdAsync(int parentNodeId)
        {
            var nodes = await FindByConditionAsync(c => c.ParentNodeId.Equals(parentNodeId));
            return nodes;
        }
        public async Task<TreeNode> GetTreeNodeByIdAsync(int treeNodeId)
        {
            var node = await FindByConditionAsync(n => n.Id.Equals(treeNodeId));
            return node.DefaultIfEmpty(new TreeNode()).FirstOrDefault();
        }
        public async Task<TreeNode> GetTreeNodeByItemIdAsync(int itemId)
        {
            var node = await FindByConditionAsync(n => n.ItemId.Equals(itemId));
            return node.DefaultIfEmpty(new TreeNode()).FirstOrDefault();
        }
        public async Task<TreeNode> GetParentNodeByChildIdAsync(int childNodeId)
        {
            var node = await FindByConditionAsync(p => p.ChildrenNodes.Any(c => c.Id.Equals(childNodeId)));
            return node.DefaultIfEmpty(new TreeNode()).FirstOrDefault();
        }
        public async Task<TreeNode> GetRootNodeByTreeNodeIdAsync(int nodeId)
        {
            var nodes = await FindByConditionAsync(r => r.Id.Equals(nodeId));
            var node = nodes.DefaultIfEmpty(new TreeNode()).FirstOrDefault();
            while (node?.ParentNode != null)
            {
                node = node.ParentNode;
            }
            return node;
        }

        public async Task CreateTreeNodeAsync(TreeNode treeNode)
        {
            Create(treeNode);
            await SaveAsync();
        }
        public async Task UpdateTreeNodeAsync(TreeNode originalNode, TreeNode updatedNode)
        {
            originalNode.Map(updatedNode);
            Update(originalNode);
            await SaveAsync();
        }
        public async Task DeleteTreeNodeAsync(TreeNode treeNode)
        {
            Delete(treeNode);
            await SaveAsync();
        }
    }
}