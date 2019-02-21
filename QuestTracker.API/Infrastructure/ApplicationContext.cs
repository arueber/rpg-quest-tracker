using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using QuestTracker.API.Entities;

namespace QuestTracker.API.Infrastructure
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationContext()
            : base("AuthContext")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<SubItem> SubItems { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<TreeNode> TreeNodes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Project User
            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new {pu.ProjectId, pu.ApplicationUserId});

            modelBuilder.Entity<ProjectUser>()
                .HasRequired(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.ApplicationUserId);

            modelBuilder.Entity<ProjectUser>()
                .HasRequired(pu => pu.Project)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<ProjectUser>()
                .HasOptional(n => n.Folder)
                .WithMany(a => a.ProjectUsers)
                .HasForeignKey(n => n.FolderId)
                .WillCascadeOnDelete(false);

            // Item
            modelBuilder.Entity<Item>()
                .HasOptional(n => n.CompletedByUser)
                .WithMany(a => a.CompletedItems)
                .HasForeignKey(n => n.CompletedByUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasOptional(n => n.AssignedUser)
                .WithMany(a => a.AssignedItems)
                .HasForeignKey(n => n.AssignedUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Item>()
                .HasMany<SubItem>(i => i.SubItems)
                .WithRequired(s => s.ParentItem)
                .HasForeignKey<int>(s => s.ParentItemId);

            // Reminders
            modelBuilder.Entity<Reminder>()
                .HasRequired(n => n.Item)
                .WithMany(a => a.Reminders)
                .HasForeignKey(n => n.ItemId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reminder>()
                .HasRequired(n => n.ApplicationUser)
                .WithMany(a => a.Reminders)
                .HasForeignKey(n => n.ApplicationUserId)
                .WillCascadeOnDelete(false);

            // SubItems
            modelBuilder.Entity<SubItem>()
                .HasRequired<Item>(n => n.ParentItem)
                .WithMany(a => a.SubItems)
                .HasForeignKey<int>(n => n.ParentItemId)
                .WillCascadeOnDelete(false);

            // Tree Nodes
            modelBuilder.Entity<TreeNode>()
                .HasRequired(n => n.Item)
                .WithOptional(i => i.TreeNode);

            modelBuilder.Entity<TreeNode>()
                .HasOptional(n => n.ParentNode);

            modelBuilder.Entity<TreeNode>()
                .HasMany(n => n.ChildrenNodes)
                .WithOptional(n => n.ParentNode);

            base.OnModelCreating(modelBuilder);
        }

    }
}