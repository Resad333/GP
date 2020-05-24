using System.Collections.Generic;

namespace Registration.Core.ApiContracts
{
    public class RedbetCustomerGetRequestResponse
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<AddressInfo> Addresses { get; set; }

        public string FavoriteFootballTeam { get; set; }
    }
}
