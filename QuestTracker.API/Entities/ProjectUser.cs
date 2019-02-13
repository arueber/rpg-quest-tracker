using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Entities
{
    public class ProjectUser
    {
        public string ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public string FolderId { get; set; }
        public virtual Folder Folder { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public bool IsOwner { get; set; }
        
        [Required]
        public bool DoNoDisturb { get; set; }
    }
}