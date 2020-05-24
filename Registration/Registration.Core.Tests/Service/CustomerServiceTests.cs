using Moq;
using Registration.Core.Entity;
using Registration.Core.Repository;
using Registration.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Registration.Core.Tests.Service
{
    public class CustomerServiceTests
    {
        [Fact]
        public void GetAll_ShouldReturnCustomers()
        {

            var fakeQueryableCustomers = new List<Customer>
            {
                CreateMrgreenCustomer(),
                CreateRedbetCustomer()
            }.AsQueryable();

            var repositoryMock = new Mock<ICustomerRepository>();
            repositoryMock.Setup(x => x.GetAll()).Returns(fakeQueryableCustomers);
            var repository = repositoryMock.Object;

            var service = new CustomerService(repository);

            var list = service.GetAll().ToArray();

            Assert.True(list.Count() > 0);
        }

        [Fact]
        public void Get_ShouldReturnCustomerWithValidId()
        {
            var id = 55;

            var customer = CreateMrgreenCustomer();
            customer.Id = id;

            var repositoryMock = new Mock<ICustomerRepository>();
            repositoryMock.Setup(x => x.Get(id)).Returns(customer);
            var repository = repositoryMock.Object;

            var service = new CustomerService(repository);
            var obj = service.Get(id);

            Assert.Equal(id, obj.Id);
        }

        [Fact]
        public void Get_ShouldNotReturnAnyCustomerWithInvalidId()
        {

            var id = 55;

            var customer = CreateMrgreenCustomer();
            customer.Id = id;

            var repositoryMock = new Mock<ICustomerRepository>();
            repositoryMock.Setup(x => x.Get(id)).Returns(customer);
            var repository = repositoryMock.Object;

            int invalidId = 77;
            var service = new CustomerService(repository);
            var obj = service.Get(invalidId);

            Assert.Null(obj);
        }

        [Fact]
        public void Add_ShouldWorkInNormalCase()
        {

            var customer = CreateMrgreenCustomer();

            var repositoryMock = new Mock<ICustomerRepository>();
            repositoryMock.Setup(x => x.Add(It.IsAny<Customer>())).Returns(customer);
            var repository = repositoryMock.Object;

            var service = new CustomerService(repository);
            var obj = service.Add(customer);

            Assert.NotNull(obj);
        }

        [Fact]
        public void Update_ShouldWorkInNormalCase()
        {
            var id = 55;

            var objBeforeUpdate = CreateMrgreenCustomer();
            objBeforeUpdate.Id = id;

            var model = CreateMrgreenCustomer();
            model.Id = id;
            model.FirstName = "NewFirstName";
            model.LastName = "NewLastName";
            model.CustomerAddresses.Clear();
            model.CustomerAddresses.Add(new Entity.CustomerAddress { Address = "Frankfurt", AddressType = Common.AddressType.REGISTRATION });
            model.CustomerBrandFields.Clear();
            model.CustomerBrandFields.Add(new Entity.CustomerField { Field = Common.FIELD.PERSONAL_NUMBER, FieldValue = "999" });

            var repositoryMock = new Mock<ICustomerRepository>();
            repositoryMock.Setup(x => x.Get(id)).Returns(objBeforeUpdate);
            repositoryMock.Setup(x => x.Save());
            var repository = repositoryMock.Object;

            var service = new CustomerService(repository);
            var objAfterUpdate = service.Update(model, id);

            Assert.Equal(objAfterUpdate.FirstName, model.FirstName);
            Assert.Equal(objAfterUpdate.LastName, model.LastName);
            Assert.Equal(objAfterUpdate.CustomerAddresses.Single(x => x.AddressType == Common.AddressType.REGISTRATION).Address,
                         model.CustomerAddresses.Single(x => x.AddressType == Common.AddressType.REGISTRATION).Address);
            Assert.Equal(objAfterUpdate.CustomerBrandFields.Single().FieldValue, model.CustomerBrandFields.Single().FieldValue);
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
