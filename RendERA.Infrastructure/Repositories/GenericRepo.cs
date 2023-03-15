using System;
using RendERA.DB.Models;


using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RendERA.Infrastructure.Repositories
{
    public class GenericRepo<T> : IRepositories.IGenericRepo<T> where T : class
    {
        public  RendERAContext context;
        private DbSet<T> _entities;
        public GenericRepo()
        {
            context = new RendERAContext();  
        }
        protected virtual RendERAContext Context
        {
            get { return context; }
            set { context = value; }
        }

        public IQueryable<T> Table => Entities;

        public GenericRepo(RendERAContext context)
        {
            Context = context;
        }
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = context.Set<T>();

                return _entities;
            }
        }
        //public AbstractRepository(AayushDBContext context)
        //{
        //    this.context = context;
        //    entities = context.Set<T>();
        //}
        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }
        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }
        public T Get(long id)
        {
            return Context.Set<T>().Find(id);
        } 
         
        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
           
            context.Set<T>().Add(entity);
             
        }
        public void AddRange(T[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            
            context.Set<T>().AddRange(entities);
            
        }
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            context.Entry(entity).State = EntityState.Modified;
            //Context.SaveChanges();
        }
        public virtual void UpdateInfo(int id, T updateEntity)
        {
            var entity = Get(id);
            context.Entry(entity).CurrentValues.SetValues(updateEntity);
            context.Entry(entity).State = EntityState.Modified;

        }
        public void Remove(int id)
        {
            var entity = Get(id);
            context.Set<T>().Remove(entity);
        }

        public void RemoveObj(T entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public void RemoveRange(T[] entities) {
            context.Set<T>().RemoveRange(entities);
        }
        public virtual void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch 
            {
                throw new ArgumentNullException("entity");
            }
        }
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);

        }
    }

}
