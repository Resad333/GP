using Registration.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Registration.Core.Service
{
    public interface ICustomerService
    {
        IQueryable<Customer> GetAll();
        Customer Add(Customer model);
        Customer Update(Customer model, object key);
        Customer Get(int id);

    }
}
