#region Using Directives



#endregion

namespace Tattoo.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private TattooEntities _context;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        protected TattooEntities DataContext
        {
            get { return _context ?? (_context = _databaseFactory.GetContext()); }
        }

        public void Commit()
        {
            DataContext.Commit();
        }
    }
}