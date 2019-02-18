using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Entities
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Revision { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public string CreatedByDeviceUDID { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}