using Newtonsoft.Json;
using Registration.Core.ApiContracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Registration.Api.Tests.Controllers
{
    public class CustomersV1ControllerTests : IClassFixture<TestFixture<Startup>>
    {

        private readonly HttpClient _client;

        public CustomersV1ControllerTests(TestFixture<Startup> fixture)
        {
            _client = fixture.Client;
        }


        [Fact]
        public async Task Post_ShouldBeSuccessfulWithValidData()
        {
            string postUrl = $@"/api/v1/CustomersV1";

            var request = new MrgreenCustomerPostRequest()
            {
                FirstName = "Test",
                LastName = "Test2",
                PersonalNumber = "99999",
                Addresses = new List<AddressInfo> { new AddressInfo { Address="Berlin", AddressType= Core.Common.AddressType.REGISTRATION },
                                                    new AddressInfo { Address="Los Angles", AddressType= Core.Common.AddressType.ACTUAL } }
            };

            var response = await _client.PostAsync(postUrl, ContentHelper.GetStringContent(request));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var rsp = JsonConvert.DeserializeObject<MrgreenCustomerPostRequest>(content);

            Assert.NotNull(rsp);

        }

        [Fact]
        public async Task Post_ShouldReturnBadRequestWithEmptyFirstName()
        {
            string postUrl = $@"/api/v1/CustomersV1";

            var request = new MrgreenCustomerPostRequest()
            {
                FirstName = "",
                LastName = "Test2",
                PersonalNumber = "99999",
                Addresses = new List<AddressInfo> { new AddressInfo { Address="Berlin", AddressType= Core.Common.AddressType.REGISTRATION },
                                                    new AddressInfo { Address="Los Angles", AddressType= Core.Common.AddressType.ACTUAL } }
            };

            var response = await _client.PostAsync(postUrl, ContentHelper.GetStringContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task Post_ShouldReturnBadRequestWithEmptyLastName()
        {
            string postUrl = $@"/api/v1/CustomersV1";

            var request = new MrgreenCustomerPostRequest()
            {
                FirstName = "ccc",
                LastName = "",
                PersonalNumber = "99999",
                Addresses = new List<AddressInfo> { new AddressInfo { Address="Berlin", AddressType= Core.Common.AddressType.REGISTRATION },
                                                    new AddressInfo { Address="Los Angles", AddressType= Core.Common.AddressType.ACTUAL } }
            };

            var response = await _client.PostAsync(postUrl, ContentHelper.GetStringContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Fact]
        public async Task Post_ShouldReturnBadRequestWithEmptyPersonalNumber()
        {
            string postUrl = $@"/api/v1/CustomersV1";

            var request = new MrgreenCustomerPostRequest()
            {
                FirstName = "xxxx",
                LastName = "Test2",
                PersonalNumber = "",
                Addresses = new List<AddressInfo> { new AddressInfo { Address="Berlin", AddressType= Core.Common.AddressType.REGISTRATION },
                                                    new AddressInfo { Address="Los Angles", AddressType= Core.Common.AddressType.ACTUAL } }
            };

            var response = await _client.PostAsync(postUrl, ContentHelper.GetStringContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Fact]
        public async Task Post_ShouldReturnBadRequestWithEmptyAddressInfo()
        {
            string postUrl = $@"/api/v1/CustomersV1";

            var request = new MrgreenCustomerPostRequest()
            {
                FirstName = "vvvv",
                LastName = "Test2",
                PersonalNumber = "99999",
                Addresses = new List<AddressInfo> { }
            };

            var response = await _client.PostAsync(postUrl, ContentHelper.GetStringContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task Post_ShouldReturnBadRequestWithInvalidAddressInfo()
        {
            string postUrl = $@"/api/v1/CustomersV1";

            var request = new MrgreenCustomerPostRequest()
            {
                FirstName = "xxxx",
                LastName = "Test2",
                PersonalNumber = "",
                Addresses = new List<AddressInfo> { new AddressInfo { Address="Berlin", AddressType= Core.Common.AddressType.REGISTRATION },
                                                    new AddressInfo { Address="Los Angles", AddressType= Core.Common.AddressType.REGISTRATION } }
            };

            var response = await _client.PostAsync(postUrl, ContentHelper.GetStringContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var request2 = new MrgreenCustomerPostRequest()
            {
                FirstName = "xxxx",
                LastName = "Test2",
                PersonalNumber = "",
                Addresses = new List<AddressInfo> { new AddressInfo { Address="Berlin", AddressType= Core.Common.AddressType.ACTUAL },
                                                    new AddressInfo { Address="Los Angles", AddressType= Core.Common.AddressType.ACTUAL } }
            };

            var response2 = await _client.PostAsync(postUrl, ContentHelper.GetStringContent(request));

            Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);

        }
    }
}
