using Registration.Core.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.Core.Entity
{
    [Table("CustomerFields")]
    public class CustomerField : BaseEntity
    {
        [Column(Order = 2)]
        public FIELD Field { get; set; }
        [Column(Order = 3)]
        public string FieldValue { get; set; }


        [Column(Order = 1)]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public CustomerField()
        {
        }
    }
}
