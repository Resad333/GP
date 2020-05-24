using System;

namespace Registration.Core.Common
{
    public class ExecuteResult<TResponse> where TResponse : class
    {
        public long ExecutionTime { get; private set; }
        public Exception Exception { get; private set; }
        public string[] BrokenList { get; private set; }
        public TResponse Response { get; private set; }


        public ExecuteResult(long executionTime, Exception exception, string[] brokenList, TResponse response)
        {
            ExecutionTime = executionTime;
            Exception = exception;
            BrokenList = brokenList;
            Response = response;
        }
    }
}
