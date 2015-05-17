#region Using Directives

using System;

#endregion

namespace Tattoo.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        TattooEntities GetContext();
    }
}