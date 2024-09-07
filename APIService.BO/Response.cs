using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIService.BO
{
    public class Response
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public Object Data { get; set; }
    }

    public class Response<T>
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}