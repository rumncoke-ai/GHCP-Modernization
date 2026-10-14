using System;

namespace LegacyShop.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IProductRepository Products { get; }
        int Complete();
    }
}
