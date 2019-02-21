using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Entities;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public interface IFolderRepository: IRepositoryBase<Folder>
    {
        Task<IEnumerable<Folder>> GetFoldersByUserIdAsync(int userId);
        Task<Folder> GetFolderByIdAsync(int folderId);

        Task CreateFolderAsync(Folder folder);
        Task UpdateFolderAsync(Folder originalFolder, Folder updatedFolder);
        Task DeleteFolderAsync(Folder folder);

    }
}
