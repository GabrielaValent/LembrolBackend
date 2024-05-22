namespace Infrastructure.DataAccess
{
    using backend_lembrol.DataAccess.Interfaces;
    using backend_lembrol.Database;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly DataContext _context;
        private bool _disposed;

        public UnitOfWork(DataContext context) => _context = context;

        public void Dispose() => Dispose(true);

        public async Task<int> SaveAsync()
        {
            try
            {
                int affectedRows = await _context.SaveChangesAsync();
                return affectedRows;
            }
            catch (Exception e)
            {
                _context.ChangeTracker.Clear();
                throw new Exception("Error persisting entities", e);
            }
        }

        public async Task BeginTransactionAsync()
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Error persisting entities", e);
            }
        }

        public async Task EndTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception("Error persisting entities", e);
            }
        }

        public async Task RollbackTransactionAsync()
        {
            _context.ChangeTracker.Clear();
            await _context.Database.RollbackTransactionAsync();
        }

        public void Detach<T>(T entity)
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }

        public void DetachRange<T>(IEnumerable<T> entities)
        {
            foreach (var item in entities)
                Detach(item);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
                _context.Dispose();

            _disposed = true;
        }
    }
}