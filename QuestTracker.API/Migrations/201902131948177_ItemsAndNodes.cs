namespace QuestTracker.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemsAndNodes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        Weight = c.Int(nullable: false),
                        PriorityFlag = c.Boolean(nullable: false),
                        URL = c.String(maxLength: 255),
                        Notes = c.String(),
                        StartDueDate = c.DateTime(nullable: false),
                        Duration = c.Time(nullable: false, precision: 7),
                        Repetition = c.Time(nullable: false, precision: 7),
                        AssignedUserId = c.String(maxLength: 128),
                        CompletionDate = c.DateTime(nullable: false),
                        CompletionApplicationUserId = c.String(maxLength: 128),
                        ProjectId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AssignedUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.CompletionApplicationUserId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.AssignedUserId)
                .Index(t => t.CompletionApplicationUserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        ProjectId = c.String(nullable: false, maxLength: 128),
                        FolderId = c.String(maxLength: 128),
                        Weight = c.Int(nullable: false),
                        IsOwner = c.Boolean(nullable: false),
                        DoNoDisturb = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUserId, t.ProjectId })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .ForeignKey("dbo.Folders", t => t.FolderId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ProjectId)
                .Index(t => t.FolderId);
            
            CreateTable(
                "dbo.Reminders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        ItemId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.SubItems",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        CompletionDate = c.DateTime(nullable: false),
                        ItemId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.TreeNodes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 100),
                        ParentNodeId = c.String(maxLength: 128),
                        ItemId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TreeNodes", t => t.ParentNodeId)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ParentNodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Folders", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TreeNodes", "Id", "dbo.Items");
            DropForeignKey("dbo.TreeNodes", "ParentNodeId", "dbo.TreeNodes");
            DropForeignKey("dbo.SubItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Reminders", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Reminders", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectUsers", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.ProjectUsers", "FolderId", "dbo.Folders");
            DropForeignKey("dbo.ProjectUsers", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Items", "CompletionApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Items", "AssignedUserId", "dbo.AspNetUsers");
            DropIndex("dbo.TreeNodes", new[] { "ParentNodeId" });
            DropIndex("dbo.TreeNodes", new[] { "Id" });
            DropIndex("dbo.SubItems", new[] { "ItemId" });
            DropIndex("dbo.Reminders", new[] { "ItemId" });
            DropIndex("dbo.Reminders", new[] { "ApplicationUserId" });
            DropIndex("dbo.ProjectUsers", new[] { "FolderId" });
            DropIndex("dbo.ProjectUsers", new[] { "ProjectId" });
            DropIndex("dbo.ProjectUsers", new[] { "ApplicationUserId" });
            DropIndex("dbo.Items", new[] { "ProjectId" });
            DropIndex("dbo.Items", new[] { "CompletionApplicationUserId" });
            DropIndex("dbo.Items", new[] { "AssignedUserId" });
            DropIndex("dbo.Folders", new[] { "ApplicationUserId" });
            DropTable("dbo.TreeNodes");
            DropTable("dbo.SubItems");
            DropTable("dbo.Reminders");
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.Projects");
            DropTable("dbo.Items");
            DropTable("dbo.Folders");
        }
    }
}
