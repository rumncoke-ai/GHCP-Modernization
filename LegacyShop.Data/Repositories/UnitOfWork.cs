namespace LegacyShop.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(context);
            Customers = new CustomerRepository(context);
            Orders = new OrderRepository(context);
            Products = new ProductRepository(context);
        }

        public ICategoryRepository Categories { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public IProductRepository Products { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
