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
    public class FolderRepository:RepositoryBase<Folder>, IFolderRepository
    {
        public FolderRepository(ApplicationContext applicationContext): base(applicationContext) { }

        public async Task<IEnumerable<Folder>> GetFoldersByUserIdAsync(int userId)
        {
            var items = await FindByConditionAsync(i => i.ProjectUsers.Any(s => s.User.Id == userId));
            return items;
        }
        public async Task<Folder> GetFolderByIdAsync(int folderId)
        {
            var folder = await FindByConditionAsync(f => f.Id.Equals(folderId));
            return folder.DefaultIfEmpty(new Folder()).FirstOrDefault();
        }

        public async Task CreateFolderAsync(Folder folder)
        {
            Create(folder);
            await SaveAsync();
        }
        public async Task UpdateFolderAsync(Folder originalFolder, Folder updatedFolder)
        {
            originalFolder.Map(updatedFolder);
            Update(originalFolder);
            await SaveAsync();
        }
        public async Task DeleteFolderAsync(Folder folder)
        {
            Delete(folder);
            await SaveAsync();
        }
    }
}