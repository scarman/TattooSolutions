#region Using Directives

using System.Collections.Generic;
using Tattoo.Common;

#endregion

namespace Tattoo.Service
{
    public interface ICrudService<T>
    {
        IEnumerable<ExecResult> Create(T entity);

        IEnumerable<ExecResult> Update(T entity);

        void Delete(string id);

        T Find(string id);
        
        IEnumerable<T> GetAll();
    }
}