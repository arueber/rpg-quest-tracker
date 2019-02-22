using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Models
{
    public class FolderDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Revision { get; set; }
    }

    public class ProjectDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Revision { get; set; }
    }
    
    public class ItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public bool PriorityFlag { get; set; }
        public string URL { get; set; }
        public string Notes { get; set; }
        public DateTime? StartDueDate { get; set; }
        public TimeDelayType? DurationType { get; set; }
        public int? DurationCount { get; set; }
        public TimeDelayType? RepetitionType { get; set; }
        public int? RepetitionCount { get; set; }
        public int Revision { get; set; }
        public string AssignedUserName { get; set; }
    }

    public class ReminderDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Revision { get; set; }
    }
}