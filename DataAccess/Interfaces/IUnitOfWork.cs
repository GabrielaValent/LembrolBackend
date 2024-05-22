namespace backend_lembrol.DataAccess.Interfaces
{
    using System.Threading.Tasks;
    public interface IUnitOfWork
    {
        void Detach<T>(T entity);

        void DetachRange<T>(IEnumerable<T> entities);

        Task<int> SaveAsync();

        Task BeginTransactionAsync();

        Task EndTransactionAsync();

        Task RollbackTransactionAsync();
    }
}