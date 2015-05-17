#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PagedList;
using Tattoo.Common.Data;

#endregion

namespace Tattoo.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void Update(T entity);
        
        void Delete(T entity);
        
        void Delete(Expression<Func<T, bool>> where);

        T GetById(params object[] id);
        
        T Get(Expression<Func<T, bool>> where);
        
        IEnumerable<T> GetAll();
        
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where);
        
        IPagedList<T> GetPage<TOrder>(PageInfo pageInfo, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order);

        IPagedList<T> GetPageOrderInverse<TOrder>(PageInfo pageInfo, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order);
    }
}