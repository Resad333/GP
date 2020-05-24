using Registration.Core.ApiContracts;
using Registration.Core.Common;
using Registration.Core.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Registration.Core.Mapper
{
    public class RedbetCustomerRequestMapper : IRedbetCustomerRequestMapper
    {

        public Customer ToDomain(RedbetCustomerPostRequest redbetCustomerPostRequest)
        {
            var entity = new Customer()
            {
                FirstName = redbetCustomerPostRequest.FirstName,
                LastName = redbetCustomerPostRequest.LastName,
                Brand = Brand.REDBET,
                CustomerAddresses = new List<CustomerAddress>(),
                CustomerBrandFields = new List<CustomerField>()
            };

            foreach (var a in redbetCustomerPostRequest.Addresses)
            {
                var address = new CustomerAddress
                {
                    Address = a.Address,
                    AddressType = a.AddressType
                };

                entity.CustomerAddresses.Add(address);
            }

            var brandData = new CustomerField
            {
                Field = FIELD.FAVORITE_FOOTBALL_TEAM,
                FieldValue = redbetCustomerPostRequest.FavoriteFootballTeam
            };
            entity.CustomerBrandFields.Add(brandData);

            return entity;
        }

        public Customer ToDomain(RedbetCustomerPutRequest redbetCustomerPutRequest)
        {
            var entity = new Customer()
            {
                Id = redbetCustomerPutRequest.Id,
                FirstName = redbetCustomerPutRequest.FirstName,
                LastName = redbetCustomerPutRequest.LastName,
                Brand = Brand.REDBET,
                CustomerAddresses = new List<CustomerAddress>(),
                CustomerBrandFields = new List<CustomerField>()
            };

            foreach (var a in redbetCustomerPutRequest.Addresses)
            {
                var address = new CustomerAddress
                {
                    Address = a.Address,
                    AddressType = a.AddressType
                };

                entity.CustomerAddresses.Add(address);
            }

            var brandData = new CustomerField
            {
                Field = FIELD.FAVORITE_FOOTBALL_TEAM,
                FieldValue = redbetCustomerPutRequest.FavoriteFootballTeam
            };
            entity.CustomerBrandFields.Add(brandData);

            return entity;
        }


        public RedbetCustomerGetRequestResponse ToGetContract(Customer entity)
        {
            if (entity == null) { return null; }

            var contract = new RedbetCustomerGetRequestResponse() { Addresses = new List<AddressInfo>() };
            contract.Id = entity.Id;
            contract.FirstName = entity.FirstName;
            contract.LastName = entity.LastName;

            contract.FavoriteFootballTeam = entity.CustomerBrandFields.Single(x => x.Field == FIELD.FAVORITE_FOOTBALL_TEAM).FieldValue;

            foreach (var a in entity.CustomerAddresses)
            {
                contract.Addresses.Add(new AddressInfo { Address = a.Address, AddressType = a.AddressType });
            }

            return contract;
        }

        public RedbetCustomerPutRequestResponse ToPutContract(Customer entity)
        {
            if (entity == null) { return null; }

            var contract = new RedbetCustomerPutRequestResponse() { Addresses = new List<AddressInfo>() };
            contract.FirstName = entity.FirstName;
            contract.LastName = entity.LastName;

            contract.FavoriteFootballTeam = entity.CustomerBrandFields.Single(x => x.Field == FIELD.FAVORITE_FOOTBALL_TEAM).FieldValue;

            foreach (var a in entity.CustomerAddresses)
            {
                contract.Addresses.Add(new AddressInfo { Address = a.Address, AddressType = a.AddressType });
            }

            return contract;
        }

        public CustomerPostRequestResponse ToPostContract(Customer entity)
        {
            return new CustomerPostRequestResponse { Id = entity.Id };
        }
    }
}
