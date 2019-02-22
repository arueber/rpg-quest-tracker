using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Models;

namespace QuestTracker.API.Entities
{
    public class Folder: IModifiedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CreatedByUserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int Revision { get; set; }

        public virtual ApplicationUser CreatedByUser { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}