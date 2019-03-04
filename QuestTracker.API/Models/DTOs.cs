using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuestTracker.API.Models
{
    #region Folder

       public class FolderDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<int> ProjectIds { get; set; }
        public string CreatedAt { get; set; }
        public int CreatedByUserId { get; set; }
        public string UpdatedAt { get; set; }
        public int Weight { get; set; }
        public int Revision { get; set; }
    }

    public class FolderCreateBindingModel
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public List<int> ProjectIds { get; set; }
    }

    public class FolderPutOrDeleteBindingModel
    {
        [MaxLength(255)]
        public string Title { get; set; }

        public List<int> ProjectIds { get; set; }

        [Required]
        public int Revision { get; set; }
    }

    #endregion

    #region Project

        public class ProjectDTO
    {
        public int Id { get; set; }
        public string CreatedAt { get; set; }
        public string Title { get; set; }
        public int Revision { get; set; }
    }

    public class ProjectCreateBindingModel
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
    }

    public class ProjectPutOrDeleteBindingModel
    {
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int Revision { get; set; }
    }

    #endregion

    #region ProjectUser

    

    #endregion

    #region Item

    public class ItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProjectId { get; set; }
        public int Weight { get; set; }
        public bool PriorityFlag { get; set; }
        public string URL { get; set; }
        public string Notes { get; set; }
        public string StartDueDate { get; set; }
        public TimeDelayType? DurationType { get; set; }
        public int? DurationCount { get; set; }
        public TimeDelayType? RepetitionType { get; set; }
        public int? RepetitionCount { get; set; }
        public bool RepetitionUsesRollingDate { get; set; }
        public int? AssignedId { get; set; }
        public string CompletedAt { get; set; }
        public string CompletedById { get; set; }
        public int Revision { get; set; }
    }

    public class ItemCreateBindingModel
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        public int? AssignedId { get; set; }
        public bool? IsCompleted { get; set; }
        public string StartDueDate { get; set; }

        public string DurationType { get; set; }

        [Range(1, int.MaxValue)]
        public int? DurationCount { get; set; }

        public string RepetitionType { get; set; }

        [Range(1, int.MaxValue)]
        public int? RepetitionCount { get; set; }
        public bool? RepetitionUsesRollingDate { get; set; }
        public bool? PriorityFlag { get; set; }
    }

    public class ItemPutOrDeleteBindingModel
    {
        public int? ProjectId { get; set; }

        [MaxLength(255)]
        public string Title { get; set; }

        public int? AssignedId { get; set; }
        public bool IsCompleted { get; set; }
        public string StartDueDate { get; set; }
        public string DurationType { get; set; }

        [Range(1, int.MaxValue)]
        public int? DurationCount { get; set; }

        public string RepetitionType { get; set; }

        [Range(1, int.MaxValue)]
        public int? RepetitionCount { get; set; }

        public bool RepetitionUsesRollingDate { get; set; }

        public bool PriorityFlag { get; set; }
        public string[] RemoveAttributes { get; set; }

        [Required]
        public int Revision { get; set; }
    }

    #endregion

    #region Reminder

public class ReminderDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ItemId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public int Revision { get; set; }
    }
    public class ReminderCreateBindingModel
    {
        [Required]
        public string Date { get; set; }

        [Required]
        public int ItemId { get; set; }

        public string CreatedByDeviceUDID { get; set; }
    }

    public class ReminderPutOrDeleteBindingModel
    {
        public string Date { get; set; }

        [Required]
        public int Revision { get; set; }
    }
    #endregion

    #region SubItem

    public class SubItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ItemId { get; set; }
        public string CreatedAt { get; set; }
        public int CreatedById { get; set; }
        public bool IsCompleted { get; set; }
        public string CompletedAt { get; set; }
        public int Revision { get; set; }
    }

    public class SubItemCreateBindingModel
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public int ItemId { get; set; }

        public bool? Completed { get; set; }
    }

    public class SubItemPutOrDeleteBindingModel
    {
        [MaxLength(255)]
        public string Title { get; set; }

        public bool? IsCompleted { get; set; }

        [Required]
        public int Revision { get; set; }
    }

    #endregion

    #region TreeNode



    #endregion

}