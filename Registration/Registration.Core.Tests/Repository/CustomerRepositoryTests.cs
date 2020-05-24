using Microsoft.EntityFrameworkCore;
using Registration.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Registration.Core.Tests.Repository
{
    public class CustomerRepositoryTests
    {
        [Fact]
        public void GetAll_ShouldReturnCustomers()
        {

            var repository = CreateRepository();

            var list = repository.GetAll().ToArray();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Get_ShouldReturnCustomerWithValidId()
        {

            var repository = CreateRepository();

            var obj = repository.Get(1);

            Assert.NotNull(obj);
        }

        [Fact]
        public void Get_ShouldNotReturnAnyCustomerWithInvalidId()
        {

            var repository = CreateRepository();

            var obj = repository.Get(90000000);

            Assert.Null(obj);
        }

        [Fact]
        public void Add_ShouldWorkInNormalCase()
        {

            var repository = CreateRepository();

            var obj = repository.Add(CreateMrgreenCustomer());

            Assert.NotNull(obj);
        }

        private CustomerRepository CreateRepository()
        {
            var dbContext = new Database.CustomerDbContext(new DbContextOptionsBuilder<Database.CustomerDbContext>().UseInMemoryDatabase().Options);

            var repository = new CustomerRepository(dbContext);

            var customer1 = CreateMrgreenCustomer();
            repository.Add(customer1);

            var customer2 = CreateRedbetCustomer();
            repository.Add(customer2);

            return repository;
        }

        private Entity.Customer CreateMrgreenCustomer()
        {
            var random = new Random();
            var number = random.Next(1, 100000000);
            var customer = new Entity.Customer
            {
                FirstName = "MrgreenCustomerF" + number,
                LastName = "MrgreenCustomerL" + number,
                Brand = Common.Brand.MRGREEN,
                CustomerAddresses = new List<Entity.CustomerAddress>() { new Entity.CustomerAddress { Address="Berlin", AddressType = Common.AddressType.REGISTRATION },
                                                                         new Entity.CustomerAddress { Address="Los Angles", AddressType= Common.AddressType.ACTUAL }},
                CustomerBrandFields = new List<Entity.CustomerField>() { new Entity.CustomerField { Field = Common.FIELD.PERSONAL_NUMBER, FieldValue = "3333" } }
            };

            return customer;
        }

        private Entity.Customer CreateRedbetCustomer()
        {
            var random = new Random();
            var number = random.Next(1, 100000000);
            var customer = new Entity.Customer
            {
                FirstName = "RedbetCustomerF" + number,
                LastName = "RedbetCustomerR" + number,
                Brand = Common.Brand.REDBET,
                CustomerAddresses = new List<Entity.CustomerAddress>() { new Entity.CustomerAddress { Address="Sao-Paulo", AddressType = Common.AddressType.REGISTRATION },
                                                                         new Entity.CustomerAddress { Address="London", AddressType= Common.AddressType.ACTUAL }},
                CustomerBrandFields = new List<Entity.CustomerField>() { new Entity.CustomerField { Field = Common.FIELD.FAVORITE_FOOTBALL_TEAM, FieldValue = "Arsenal" } }
            };

            return customer;
        }
    }
}
