using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QuestTracker.API.Infrastructure;
using QuestTracker.API.Models;

namespace QuestTracker.API.Entities
{
    public class Folder
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int Weight { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
    }
}