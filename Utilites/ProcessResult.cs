using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Utilites
{
    public class ProcessResult<T>
    {
        public ProcessResult(string methodName)
        {
            MethodName = methodName;
        }
        public ProcessResult()
        {

        }
        public bool IsSucceeded { get; set; }
        public string MethodName { get; set; }
        public string Status { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
       public int? TotalUserCount { get; set; }
    }
}
