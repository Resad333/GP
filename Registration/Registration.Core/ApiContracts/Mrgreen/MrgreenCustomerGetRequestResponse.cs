using Registration.Core.Common;
using System.Collections.Generic;

namespace Registration.Core.ApiContracts
{
    public class MrgreenCustomerGetRequestResponse
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<AddressInfo> Addresses { get; set; }

        public string PersonalNumber { get; set; }
    }

    public class AddressInfo
    {
        public string Address { get; set; }

        public AddressType AddressType { get; set; }
    }
}
