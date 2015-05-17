#region Using Directives

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PagedList;
using Tattoo.Common.Data;

#endregion

namespace Tattoo.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private readonly IDbSet<T> _dbSet;
        private TattooEntities _context;

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbSet = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected TattooEntities DataContext
        {
            get { return _context ?? (_context = DatabaseFactory.GetContext()); }
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
                _dbSet.Remove(obj);
        }

        public virtual T GetById(params object[] id)
        {
            return _dbSet.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }

        /// <summary>
        ///     Return a paged list of entities
        /// </summary>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="pageInfo">Which page to retrieve</param>
        /// <param name="where">Where clause to apply</param>
        /// <param name="order">Order by to apply</param>
        /// <returns></returns>
        public virtual IPagedList<T> GetPage<TOrder>(PageInfo pageInfo, Expression<Func<T, bool>> where,
            Expression<Func<T, TOrder>> order)
        {
            var results = _dbSet.OrderBy(order).Where(where).GetPage(pageInfo).ToList();
            var total = _dbSet.Count(where);
            return new StaticPagedList<T>(results, pageInfo.PageNumber, pageInfo.PageSize, total);
        }
        public virtual IPagedList<T> GetPageOrderInverse<TOrder>(PageInfo pageInfo, Expression<Func<T, bool>> where,
            Expression<Func<T, TOrder>> order)
        {
            var results = _dbSet.OrderByDescending(order).Where(where).GetPage(pageInfo).ToList();
            var total = _dbSet.Count(where);
            return new StaticPagedList<T>(results, pageInfo.PageNumber, pageInfo.PageSize, total);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).FirstOrDefault();
        }
    }
}