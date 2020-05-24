using Registration.Core.Entity;
using Registration.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Registration.Core.Service
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Customer> GetAll() => _repository.GetAll();

        public Customer Add(Customer model)
        {
            return _repository.Add(model);
        }

        public Customer Update(Customer model, object key)
        {
            var modifiedDate = DateTime.Now;

            var obj = _repository.Get(model.Id);

            obj.FirstName = model.FirstName;
            obj.LastName = model.LastName;
            obj.Brand = model.Brand;
            obj.ModifiedDate = modifiedDate;

            ModifyAddresses(obj, model, modifiedDate);
            ModifyFields(obj, model, modifiedDate);

            _repository.Save();

            return obj;
        }

        public Customer Get(int id)
        {
            return _repository.Get(id);
        }

        private void ModifyAddresses(Customer obj, Customer model, DateTime modifiedDate)
        {

            var customerAddresses = obj.CustomerAddresses;
            var existingAddressTypes = customerAddresses.Select(x => x.AddressType).ToArray();

            foreach (var a in model.CustomerAddresses)
            {
                if (existingAddressTypes.Contains(a.AddressType))
                {
                    var customerAddress = customerAddresses.Single(c => c.AddressType == a.AddressType);

                    customerAddress.ModifiedDate = modifiedDate;
                    customerAddress.Address = a.Address;
                }
                else
                {
                    obj.CustomerAddresses.Add(a);
                }
            }
        }

        private void ModifyFields(Customer obj, Customer model, DateTime modifiedDate)
        {

            var customerBrands = obj.CustomerBrandFields;
            var existingFields = customerBrands.Select(x => x.Field).ToArray();

            foreach (var a in model.CustomerBrandFields)
            {
                if (existingFields.Contains(a.Field))
                {
                    var customerBrand = customerBrands.Single(c => c.Field == a.Field);

                    customerBrand.ModifiedDate = modifiedDate;
                    customerBrand.FieldValue = a.FieldValue;
                }
                else
                {
                    obj.CustomerBrandFields.Add(a);
                }
            }
        }
    }
}
