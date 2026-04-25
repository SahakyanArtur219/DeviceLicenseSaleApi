using DeviceLicenseSaleApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeviceLicenseSaleApi.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public EfUnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public ITransactionScope BeginTransaction()
        {
            return new EfTransactionScope(_context.Database.BeginTransaction());
        }

        private sealed class EfTransactionScope : ITransactionScope
        {
            private readonly IDbContextTransaction _transaction;

            public EfTransactionScope(IDbContextTransaction transaction)
            {
                _transaction = transaction;
            }

            public void Commit()
            {
                _transaction.Commit();
            }

            public void Rollback()
            {
                _transaction.Rollback();
            }

            public void Dispose()
            {
                _transaction.Dispose();
            }
        }
    }
}
