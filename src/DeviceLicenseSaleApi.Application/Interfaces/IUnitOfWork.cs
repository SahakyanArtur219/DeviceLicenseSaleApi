namespace DeviceLicenseSaleApi.Services.Interfaces
{
    public interface IUnitOfWork
    {
        ITransactionScope BeginTransaction();
    }

    public interface ITransactionScope : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
