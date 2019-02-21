using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using QuestTracker.API.Entities;
using QuestTracker.API.Entities.Extensions;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var projects = await FindAllAsync();
            return projects;
        }

        public async Task<Project> GetProjectByIdAsync(int projectId)
        {
            var project = await FindByConditionAsync(p => p.Id.Equals(projectId));
            return project.DefaultIfEmpty(new Project()).FirstOrDefault();
        }

        public async Task CreateProjectAsync(Project project)
        {
            Create(project);
            await SaveAsync();
        }

        public async Task UpdateProjectAsync(Project originalProject, Project updatedProject)
        {
            originalProject.Map(updatedProject);
            Update(originalProject);
            await SaveAsync();
        }

        public async Task DeleteProjectAsync(Project project)
        {
            Delete(project);
            await SaveAsync();
        }
    }
}