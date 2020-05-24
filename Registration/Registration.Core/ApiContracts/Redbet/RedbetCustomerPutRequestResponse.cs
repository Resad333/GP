﻿using System.Collections.Generic;

namespace Registration.Core.ApiContracts
{
    public class RedbetCustomerPutRequestResponse
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<AddressInfo> Addresses { get; set; }

        public string FavoriteFootballTeam { get; set; }
    }
}
