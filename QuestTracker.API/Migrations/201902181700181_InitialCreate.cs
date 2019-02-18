namespace QuestTracker.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        JoinDate = c.DateTime(nullable: false),
                        PSK = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PhotoURL = c.String(maxLength: 255),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Weight = c.Int(nullable: false),
                        PriorityFlag = c.Boolean(nullable: false),
                        URL = c.String(maxLength: 255),
                        Notes = c.String(),
                        StartDueDate = c.DateTime(),
                        Duration = c.Time(precision: 7),
                        Repetition = c.Time(precision: 7),
                        AssignedUserId = c.Int(),
                        CompletionDate = c.DateTime(),
                        CompletionApplicationUserId = c.Int(),
                        ProjectId = c.Int(nullable: false),
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
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectUsers",
                c => new
                    {
                        ApplicationUserId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        FolderId = c.Int(),
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
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ApplicationUserId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
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
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CompletionDate = c.DateTime(),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.TreeNodes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ParentNodeId = c.Int(),
                        ItemId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TreeNodes", t => t.ParentNodeId)
                .ForeignKey("dbo.Items", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.ParentNodeId);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Folders", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
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
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
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
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Folders", new[] { "ApplicationUserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.TreeNodes");
            DropTable("dbo.SubItems");
            DropTable("dbo.Reminders");
            DropTable("dbo.ProjectUsers");
            DropTable("dbo.Projects");
            DropTable("dbo.Items");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Folders");
            DropTable("dbo.Clients");
        }
    }
}
