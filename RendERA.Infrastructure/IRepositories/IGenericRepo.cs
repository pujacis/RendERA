using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RendERA.Infrastructure.IRepositories

{
    public interface IGenericRepo<T> : IDisposable where T : class
    {
        IQueryable<T> Table { get; }
        IEnumerable<T> GetAll();
        T Get(int id);
        T Get(long id);
        void RemoveObj(T entity);
        void Add(T entity);
        void AddRange(T[] entities);
        void Update(T entity);
        void UpdateInfo(int id, T entity);
        void Remove(int id);
        void RemoveRange(T[] entities);

        void Save();

    }
 }
