using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using QuestTracker.API.Entities;
using QuestTracker.API.Entities.Extensions;
using QuestTracker.API.Models;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public class ProjectUserRepository: RepositoryBase<ProjectUser>, IProjectUserRepository
    {
        public ProjectUserRepository(ApplicationContext applicationContext): base(applicationContext) { }


        public async Task<IEnumerable<ProjectUser>> GetProjectsByFolderIdAsync(int folderId)
        {
            var projectUsers = await FindByConditionAsync(pu => pu.FolderId.Equals(folderId));
            return projectUsers;
        }

        public async Task<IEnumerable<ProjectUser>> GetProjectsByUserIdAsync(int userId)
        {
            var projectUsers = await FindByConditionAsync(pu => pu.ApplicationUserId.Equals(userId));
            return projectUsers;
        }

        public async Task<IEnumerable<ProjectUser>> GetUsersByProjectIdAsync(int projectId)
        {
            var projectUsers = await FindByConditionAsync(pu => pu.ProjectId.Equals(projectId));
            return projectUsers;
        }

        public async Task<ProjectUser> GetProjectUserByProjectIdAndUserIdAsync(int projectId, int userId)
        {
            var projectUser =
                await FindByConditionAsync(pu => pu.ProjectId.Equals(projectId) && pu.ApplicationUserId.Equals(userId));
            return projectUser.DefaultIfEmpty(new ProjectUser()).FirstOrDefault();
        }

        public async Task CreateProjectUserAsync(ProjectUser projectUser)
        {
            Create(projectUser);
            await SaveAsync();
        }

        public async Task UpdateProjectUserAsync(ProjectUser originalProjectUser, ProjectUser updatedProjectUser)
        {
            originalProjectUser.Map(updatedProjectUser);
            Update(originalProjectUser);
            await SaveAsync();
        }

        public async Task DeleteProjectUserAsync(ProjectUser projectUser)
        {
            Delete(projectUser);
            await SaveAsync();
        }
    }
}