using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public interface IRepositoryWrapper
    {
        IFolderRepository Folder { get; }
        IProjectRepository Project { get; }
        IProjectUserRepository ProjectUser { get; }
        IItemRepository Item { get; }
        IReminderRepository Reminder { get; }
        ISubItemRepository SubItem { get; }
        ITreeNodeRepository TreeNode { get; }
    }
}
