using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Entities;
using QuestTracker.API.Models;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public interface IProjectUserRepository: IRepositoryBase<ProjectUser>
    {
        Task<IEnumerable<ProjectUser>> GetProjectsByFolderIdAsync(int folderId);
        Task<IEnumerable<ProjectUser>> GetProjectsByUserIdAsync(int userId);
        Task<IEnumerable<ProjectUser>> GetUsersByProjectIdAsync(int projectId);
        Task<ProjectUser> GetProjectUserByProjectIdAndUserIdAsync(int projectId, int userId);

        Task CreateProjectUserAsync(ProjectUser projectUser);
        Task UpdateProjectUserAsync(ProjectUser originalProjectUser, ProjectUser updatedProjectUser);
        Task DeleteProjectUserAsync(ProjectUser projectUser);
       
    }
}
