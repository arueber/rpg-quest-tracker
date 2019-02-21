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
        [Required]
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        [Required]
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int? FolderId { get; set; }
        public virtual Folder Folder { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public bool IsOwner { get; set; }
        
        [Required]
        public bool DoNoDisturb { get; set; }

        [Required]
        public bool Accepted { get; set; }

        [Required]
        public int Revision { get; set; }
    }
}