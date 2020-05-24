using Registration.Core.Database;
using Registration.Core.Entity;

namespace Registration.Core.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CustomerDbContext context)
           : base(context)
        {
        }
    }
}
