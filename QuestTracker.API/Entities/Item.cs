﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Models;

namespace QuestTracker.API.Entities
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

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

        public DateTime? StartDueDate { get; set; }

        public TimeDelayType? DurationType { get; set; }
        public int? DurationCount { get; set; }

        public TimeDelayType? RepetitionType { get; set; }
        public int? RepetitionCount { get; set; }

        [Required]
        public int Revision { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }

        public int? AssignedUserId { get; set; }
        public virtual ApplicationUser AssignedUser { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int? CompletedByUserId { get; set; }
        public virtual ApplicationUser CompletedByUser { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<SubItem> SubItems { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }

        public virtual TreeNode TreeNode { get; set; }

    }
}