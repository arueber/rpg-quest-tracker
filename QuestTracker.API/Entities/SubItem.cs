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

        public DateTime? CompletionDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public int CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int Revision { get; set; }

        [Required]
        public int ParentItemId { get; set; }
        public virtual Item ParentItem { get; set; }
    }
}