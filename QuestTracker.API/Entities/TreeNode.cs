using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities
{
    public class TreeNode
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
       
        public string ParentNodeId { get; set; }
        public virtual TreeNode ParentNode { get; set; }

        [Required]
        public string ItemId { get; set; }
        public virtual Item Item { get; set; }

        public virtual ICollection<TreeNode> ChildrenNodes { get; set; }
    }
}