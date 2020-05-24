using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Registration.Core.Abstraction;
using Registration.Core.ApiContracts;
using Registration.Core.Common;
using Registration.Core.Mapper;
using Registration.Core.Service;
using Registration.Core.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Registration.Api.Controllers
{
   
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    [ApiController]
    public class CustomersV2Controller : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IRedbetCustomerRequestMapper _customerRequestMapper;
        private readonly IRedbetCustomerRequestValidator _customerRequestValidator;
        private readonly ICustomerService _customerService;
        private readonly Brand _brand = Brand.REDBET;


        public CustomersV2Controller(ICustomerService customerService, IRedbetCustomerRequestMapper customerRequestMapper,
            IRedbetCustomerRequestValidator customerRequestValidator, ILogger logger)
        {
            _customerService = customerService;
            _customerRequestMapper = customerRequestMapper;
            _customerRequestValidator = customerRequestValidator;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Get()
        {
            _logger.Information("get all request received");

            var request = new CustomerGetRequest() { };

            var executeResult = RequestProcessor<CustomerGetRequest, List<RedbetCustomerGetRequestResponse>>
                                 .Create(_logger, request, Request)
                                 .Execute(() =>
                                 {
                                     var list = new List<RedbetCustomerGetRequestResponse>();

                                     var customers = _customerService.GetAll().Where(c => c.Brand == _brand).ToList();
                                     foreach (var customer in customers)
                                     {
                                         list.Add(_customerRequestMapper.ToGetContract(customer));
                                     }

                                     return list;

                                 });

            _logger.Information("process result", new { ExecuteResult = executeResult });

            bool hasInternalServerError = executeResult.Exception != null;
            if (hasInternalServerError)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "While processing request error occured");
            }

            var response = executeResult.Response;
            if (response == null || response?.Count == 0)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No data found");
            }

            _logger.Information("response", response);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            _logger.Information("get request received", id);

            var request = new CustomerGetRequest { Id = id };

            var executeResult = RequestProcessor<CustomerGetRequest, RedbetCustomerGetRequestResponse>
                                 .Create(_logger, request, Request)
                                 .Execute(() =>
                                 {
                                     var customer = _customerService.Get(id);

                                     if (customer==null || customer.Brand != _brand) { return null; }

                                     return _customerRequestMapper.ToGetContract(customer);
                                 });

            _logger.Information("process result", new { ExecuteResult = executeResult });

            bool hasInternalServerError = executeResult.Exception != null;
            if (hasInternalServerError)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "While processing request error occured");
            }

            var response = executeResult.Response;
            if (response == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No data found");
            }

            _logger.Information("response", response);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] RedbetCustomerPutRequest request)
        {
            request.Id = id;

            _logger.Information("put request received", new { Request = request });

            var executeResult = RequestProcessor<RedbetCustomerPutRequest, RedbetCustomerPutRequestResponse>
                                 .Create(_logger, request, Request)
                                 .AddRequestValidator(_customerRequestValidator.ValidatePutRequest)
                                 .Execute(() =>
                                 {
                                     var customer = _customerService.Get(id);

                                     if (customer == null || customer.Brand != _brand) { return null; }

                                     var domain = _customerService.Update(_customerRequestMapper.ToDomain(request), id);

                                     var contract = _customerRequestMapper.ToPutContract(domain);

                                     return contract;
                                 });

            _logger.Information("process result", new { ExecuteResult = executeResult });

            bool hasInternalServerError = executeResult.Exception != null;
            if (hasInternalServerError)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "While processing request error occured");
            }

            bool isBadRequest = executeResult.BrokenList?.Length > 0;
            if (isBadRequest)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, string.Join(Environment.NewLine, executeResult.BrokenList));
            }

            var response = executeResult.Response;
            if (response == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No data found");
            }

            _logger.Information("response", response);

            return Ok(response);
        }

        [HttpPost]
        public ActionResult Post([FromBody]RedbetCustomerPostRequest request)
        {
            _logger.Information("post request received", new { Request = request });

            var executeResult = RequestProcessor<RedbetCustomerPostRequest, CustomerPostRequestResponse>
                                 .Create(_logger, request, Request)
                                 .AddRequestValidator(_customerRequestValidator.ValidatePostRequest)
                                 .Execute(() =>
                                 {
                                     var domain = _customerService.Add(_customerRequestMapper.ToDomain(request));

                                     var contract = _customerRequestMapper.ToPostContract(domain);

                                     return contract;
                                 });

            _logger.Information("process result", new { ExecuteResult = executeResult });

            bool hasInternalServerError = executeResult.Exception != null;
            if (hasInternalServerError)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "While processing request error occured");
            }

            bool isBadRequest = executeResult.BrokenList?.Length > 0;
            if (isBadRequest)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, string.Join(Environment.NewLine, executeResult.BrokenList));
            }

            var response = executeResult.Response;

            _logger.Information("response", response);

            return Created(new Uri(Request.GetEncodedUrl() + "/" + response.Id), request);
        }


    }
}
