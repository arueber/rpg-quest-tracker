using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Entities
{
    public class Item
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public bool PriorityFlag { get; set; }

        [MaxLength(255)]
        public string URL { get; set; }

        public string Notes { get; set; }

        public DateTime StartDueDate { get; set; }

        public TimeSpan Duration { get; set; }

        public TimeSpan Repetition { get; set; }

        public string AssignedUserId { get; set; }
        public virtual ApplicationUser AssignedUser { get; set; }

        public DateTime CompletionDate { get; set; }

        public string CompletionApplicationUserId { get; set; }
        public virtual ApplicationUser CompletionApplicationUser { get; set; }

        [Required]
        public string ProjectId { get; set; }
        public virtual Project Projects { get; set; }

        public virtual ICollection<SubItem> SubItems { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }

        public virtual TreeNode TreeNode { get; set; }

    }
}