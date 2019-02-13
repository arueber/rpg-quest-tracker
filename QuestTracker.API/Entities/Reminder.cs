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
        public string Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public string ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}