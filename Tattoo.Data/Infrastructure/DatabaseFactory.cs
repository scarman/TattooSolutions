#region Using Directives

#endregion

namespace Tattoo.Data.Infrastructure
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        private TattooEntities _context;

        public TattooEntities GetContext()
        {
            return _context ?? (_context = new TattooEntities());
        }

        protected override void DisposeCore()
        {
            //if (_context != null)
            //    _context.Dispose();
        }
    }
}