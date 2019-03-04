using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Entities
{
    public class SubItem: IModifiedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public DateTime? CompletedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public int CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int Revision { get; set; }

        [Required]
        public int ParentItemId { get; set; }
        public virtual Item ParentItem { get; set; }

        public SubItem() { }

        public SubItem(string title, int parentItemId, int userId, bool completed)
        {
            Title = title;
            ParentItemId = parentItemId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            CreatedByUserId = userId;
            UpdatedAt = DateTime.UtcNow;
            if (completed)
            {
                CompletedAt = DateTime.UtcNow;
            }
            Revision = 0;
        }
    }
}