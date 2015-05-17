#region Using Directives

using System.Linq;

#endregion

namespace Tattoo.Common.Data
{
    public static class PagingExtensions
    {
        /// <summary>
        ///     Extend IQueryable to simplify access to skip and take methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageInfo"></param>
        /// <returns>IQueryable with Skip and Take having been performed</returns>
        public static IQueryable<T> GetPage<T>(this IQueryable<T> queryable, PageInfo pageInfo)
        {
            return queryable.Skip(pageInfo.Skip).Take(pageInfo.PageSize);
        }
    }
}