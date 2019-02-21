using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace QuestTracker.API.Infrastructure
{
    public abstract class RepositoryBase<T>: IRepositoryBase<T> where T : class
    {
        protected ApplicationContext ApplicationContext { get; set; }

        public RepositoryBase(ApplicationContext applicationContext)
        {
            this.ApplicationContext = applicationContext;
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await this.ApplicationContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await this.ApplicationContext.Set<T>().Where(expression).ToListAsync();
        }

        public void Create(T entity)
        {
            this.ApplicationContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.ApplicationContext.Set<T>().AddOrUpdate(entity);
        }

        public void Delete(T entity)
        {
            this.ApplicationContext.Set<T>().Remove(entity);
        }

        public async Task SaveAsync()
        {
            await this.ApplicationContext.SaveChangesAsync();
        }
    }
}