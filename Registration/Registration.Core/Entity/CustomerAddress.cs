using Registration.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.Core.Entity
{
    [Table("CustomerAddresses")]
    public class CustomerAddress : BaseEntity
    {
        [Column(Order = 2)]
        public string Address { get; set; }
        [Column(Order = 3)]
        public AddressType AddressType { get; set; } = AddressType.REGISTRATION;


        [Column(Order = 1)]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public CustomerAddress()
        {
        }
    }
}
