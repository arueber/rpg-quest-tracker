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
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int Revision { get; set; }
       
        public int? ParentNodeId { get; set; }
        public virtual TreeNode ParentNode { get; set; }

        [Required]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public virtual ICollection<TreeNode> ChildrenNodes { get; set; }
    }
}