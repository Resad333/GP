using Registration.Core.ApiContracts;
using System.Linq;

namespace Registration.Core.Validator
{
    public class RedbetCustomerRequestValidator : IRedbetCustomerRequestValidator
    {
        public string ValidatePostRequest(RedbetCustomerPostRequest request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
            {
                return "Illegal value received for FirstName";
            }
            if (string.IsNullOrEmpty(request.LastName))
            {
                return "Illegal value received for LastName";
            }
            if (request.Addresses == null || request.Addresses.Count == 0)
            {
                return "Illegal value received for Address";
            }

            if (request.Addresses.Select(x => x.AddressType).Distinct().Count() != request.Addresses.Count())
            {
                return "AddressTypes must be unique";
            }

            foreach (var a in request.Addresses)
            {
                if (string.IsNullOrEmpty(a.Address))
                {
                    return "Empty value received for Address";
                }
            }
            if (string.IsNullOrEmpty(request.FavoriteFootballTeam))
            {
                return "Illegal value received for PersonalNumber";
            }

            return null;
        }

        public string ValidatePutRequest(RedbetCustomerPutRequest request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
            {
                return "Illegal value received for FirstName";
            }
            if (string.IsNullOrEmpty(request.LastName))
            {
                return "Illegal value received for LastName";
            }
            if (request.Addresses == null || request.Addresses.Count == 0)
            {
                return "Illegal value received for Address";
            }
            if (request.Addresses.Select(x => x.AddressType).Distinct().Count() != request.Addresses.Count())
            {
                return "AddressTypes must be unique";
            }
            foreach (var a in request.Addresses)
            {
                if (string.IsNullOrEmpty(a.Address))
                {
                    return "Empty value received for Address";
                }
            }

            if (string.IsNullOrEmpty(request.FavoriteFootballTeam))
            {
                return "Illegal value received for PersonalNumber";
            }

            return null;
        }
    }
}
