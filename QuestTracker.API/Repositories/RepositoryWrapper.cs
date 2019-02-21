using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestTracker.API.Infrastructure;

namespace QuestTracker.API.Repositories
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private ApplicationContext _applicationContext;
        private IFolderRepository _folder;
        private IProjectRepository _project;
        private IProjectUserRepository _projectUser;
        private IItemRepository _item;
        private IReminderRepository _reminder;
        private ISubItemRepository _subItem;
        private ITreeNodeRepository _treeNode;


        public IFolderRepository Folder
        {
            get
            {
                if (_folder == null)
                {
                    _folder = new FolderRepository(_applicationContext);
                }
                return _folder;
            }
        }
        public IProjectRepository Project
        {
            get
            {
                if (_project == null)
                {
                    _project = new ProjectRepository(_applicationContext);
                }
                return _project;
            }
        }

        public IProjectUserRepository ProjectUser
        {
            get
            {
                if (_projectUser == null)
                {
                    _projectUser = new ProjectUserRepository(_applicationContext);
                }
                return _projectUser;
            }
        }

        public IItemRepository Item
        {
            get
            {
                if (_item == null)
                {
                    _item = new ItemRepository(_applicationContext);
                }
                return _item;
            }
        }

        public IReminderRepository Reminder
        {
            get
            {
                if (_reminder == null)
                {
                    _reminder = new ReminderRepository(_applicationContext);
                }
                return _reminder;
            }
        }

        public ISubItemRepository SubItem
        {
            get
            {
                if (_subItem == null)
                {
                    _subItem = new SubItemRepository(_applicationContext);
                }
                return _subItem;
            }
        }

        public ITreeNodeRepository TreeNode
        {
            get
            {
                if (_treeNode == null)
                {
                    _treeNode = new TreeNodeRepository(_applicationContext);
                }
                return _treeNode;
            }
        }


        public RepositoryWrapper(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
    }
}