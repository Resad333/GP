using Registration.Core.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.Core.Entity
{
    [Table("Customers")]
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Brand Brand { get; set; }

        public virtual IList<CustomerAddress> CustomerAddresses { get; set; }
        public virtual IList<CustomerField> CustomerBrandFields { get; set; }

        public Customer()
        {
        }
    }
}
