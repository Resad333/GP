using System.Collections.Generic;

namespace Registration.Core.ApiContracts
{
    public class MrgreenCustomerPostRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<AddressInfo> Addresses { get; set; }

        public string PersonalNumber { get; set; }
    }
}
