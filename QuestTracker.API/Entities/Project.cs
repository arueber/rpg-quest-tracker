using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int Revision { get; set; }

        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}