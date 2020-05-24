using Registration.Core.ApiContracts;
using Registration.Core.Common;
using Registration.Core.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Registration.Core.Mapper
{
    public class MrgreenCustomerRequestMapper : IMrgreenCustomerRequestMapper
    {
        public Customer ToDomain(MrgreenCustomerPostRequest mrgreenCustomerPostRequest)
        {
            var entity = new Customer()
            {
                Brand = Brand.MRGREEN,
                FirstName = mrgreenCustomerPostRequest.FirstName,
                LastName = mrgreenCustomerPostRequest.LastName,
                CustomerAddresses = new List<CustomerAddress>(),
                CustomerBrandFields = new List<CustomerField>()
            };

            foreach (var a in mrgreenCustomerPostRequest.Addresses)
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
                Field = FIELD.PERSONAL_NUMBER,
                FieldValue = mrgreenCustomerPostRequest.PersonalNumber
            };
            entity.CustomerBrandFields.Add(brandData);

            return entity;
        }

        public Customer ToDomain(MrgreenCustomerPutRequest mrgreenCustomerPutRequest)
        {
            var entity = new Customer()
            {
                Id = mrgreenCustomerPutRequest.Id,
                Brand = Brand.MRGREEN,
                FirstName = mrgreenCustomerPutRequest.FirstName,
                LastName = mrgreenCustomerPutRequest.LastName,
                CustomerAddresses = new List<CustomerAddress>(),
                CustomerBrandFields = new List<CustomerField>()
            };

            foreach (var a in mrgreenCustomerPutRequest.Addresses)
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
                Field = FIELD.PERSONAL_NUMBER,
                FieldValue = mrgreenCustomerPutRequest.PersonalNumber
            };
            entity.CustomerBrandFields.Add(brandData);

            return entity;
        }

        public MrgreenCustomerGetRequestResponse ToGetContract(Customer entity)
        {
            if (entity == null) { return null; }

            var contract = new MrgreenCustomerGetRequestResponse() { Addresses = new List<AddressInfo>() };
            contract.Id = entity.Id;
            contract.FirstName = entity.FirstName;
            contract.LastName = entity.LastName;

            contract.PersonalNumber = entity.CustomerBrandFields.Single(x => x.Field == FIELD.PERSONAL_NUMBER).FieldValue;

            foreach (var a in entity.CustomerAddresses)
            {
                contract.Addresses.Add(new AddressInfo { Address = a.Address, AddressType = a.AddressType });
            }

            return contract;
        }

        public MrgreenCustomerPutRequestResponse ToPutContract(Customer entity)
        {
            if (entity == null) { return null; }

            var contract = new MrgreenCustomerPutRequestResponse() { Addresses = new List<AddressInfo>() };
            contract.FirstName = entity.FirstName;
            contract.LastName = entity.LastName;

            contract.PersonalNumber = entity.CustomerBrandFields.Single(x => x.Field == FIELD.PERSONAL_NUMBER).FieldValue;

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
