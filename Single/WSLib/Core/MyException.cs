using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSLib.Core
{
    public class MyException
    {
        public string Message { get; private set; }
        public int Code { get; private set; }
        public MyException(string message,int code)
        {
            Message= message;
            Code = code;
        }
    }
}
