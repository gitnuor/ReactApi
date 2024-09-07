using System;
using System.Collections.Generic;
using System.Text;


namespace APIService.DAL.Helper
{
    public class StaticContext
    {
        public static class Code
        {
            public static string Success = "000";
            public static string Warning = "666";
            public static string Error = "999";
        }
        public static class MessageType
        {
            public static string Success = "success";
            public static string Warning = "warning";
            public static string Error = "error";
            public static string Info = "info";
        }
        public static class Message
        {
            public static string Success = "Data saved successfully.";
            public static string Error = "Data saved failed.";
        }

    }
}

