using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Entities
{
    public class SubItem
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CompletionDate { get; set; }

        public string ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}