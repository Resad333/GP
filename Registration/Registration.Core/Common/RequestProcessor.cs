using Microsoft.AspNetCore.Http;
using Registration.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Registration.Core.Common
{/// <summary>
/// All logic must be executed Execute method of generic RequestProcessor class which accepts func delegate.
/// Exceptions will be handled  inside Execute method.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
    public class RequestProcessor<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly HttpRequest _httpRequest;
        private readonly TRequest _request;
        private readonly List<Func<TRequest, string>> _requestValidators;
        private readonly List<Func<TResponse, string>> _responseValidators;
        private readonly ILogger _logger;

        private RequestProcessor(ILogger logger, TRequest request, HttpRequest httpRequest)
        {
            _logger = logger;
            _httpRequest = httpRequest;
            _request = request;
            _requestValidators = new List<Func<TRequest, string>>();
            _responseValidators = new List<Func<TResponse, string>>();
        }

        private List<string> ProcessRequestValidators()
        {
            var brokenList = new List<string>();

            if (_requestValidators.Count == 0)
            {
                return brokenList;
            }

            foreach (var requestValidatorFunc in _requestValidators)
            {
                string validationResult = requestValidatorFunc(_request);

                if (!string.IsNullOrEmpty(validationResult))
                {
                    brokenList.Add(validationResult);
                }
            }

            return brokenList;
        }

        private List<string> ProcessResponseValidators(TResponse response)
        {
            var brokenList = new List<string>();

            if (_responseValidators.Count == 0)
            {
                return brokenList;
            }

            foreach (var responseValidatorFunc in _responseValidators)
            {
                string validationResult = responseValidatorFunc(response);

                if (!string.IsNullOrEmpty(validationResult))
                {
                    brokenList.Add(validationResult);
                }
            }

            return brokenList;
        }

        public static RequestProcessor<TRequest, TResponse> Create(ILogger logger, TRequest request, HttpRequest httpRequest = null)
        {
            return new RequestProcessor<TRequest, TResponse>(logger, request, httpRequest);
        }

        public RequestProcessor<TRequest, TResponse> AddRequestValidator(Func<TRequest, string> requestValidator)
        {
            _requestValidators.Add(requestValidator);

            return this;
        }

        public RequestProcessor<TRequest, TResponse> AddResponseValidator(Func<TResponse, string> responseValidator)
        {
            _responseValidators.Add(responseValidator);

            return this;
        }

        public ExecuteResult<TResponse> Execute(Func<TResponse> createResponseFunc)
        {
            TResponse response = null;
            long executionTime;
            List<string> brokenList = null;
            Exception exception = null;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                brokenList = ProcessRequestValidators();
                if (brokenList.Count == 0)
                {
                    response = createResponseFunc();

                    brokenList = ProcessResponseValidators(response);
                }
            }
            catch (Exception ex)
            {
                exception = ex;

                _logger.Error("error occured while processing api request", new
                {
                    Request = _request,
                    Response = response,
                    ExceptionType = ex.GetType().Name,
                    ExceptionMessage = ex.Message,
                    ExceptionStackTrace = ex.StackTrace
                });

            }
            finally
            {
                watch.Stop();
                executionTime = watch.ElapsedMilliseconds;

                _httpRequest?.HttpContext.Response.Headers.Add("x-process-execution-time", executionTime.ToString());
            }

            return new ExecuteResult<TResponse>(executionTime, exception, brokenList?.ToArray(), response);
        }
    }
}