using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities.Extensions
{
    public static class TreeNodeExtensions
    {
        public static void Map(this TreeNode dbTreeNode, TreeNode treeNode)
        {
            dbTreeNode.Name = treeNode.Name;
            dbTreeNode.CreatedAt = treeNode.CreatedAt;
            dbTreeNode.UpdatedAt = treeNode.UpdatedAt;
            dbTreeNode.Revision = treeNode.Revision;
            dbTreeNode.ParentNodeId = treeNode.ParentNodeId;
            dbTreeNode.ItemId = treeNode.ItemId;

        }
    }
}