namespace Tattoo.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}