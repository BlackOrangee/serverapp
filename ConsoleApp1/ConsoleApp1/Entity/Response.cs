using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Entity
{
    public class Response<T>
    {
        public T Obj { get; set; }

        public string errorMessage { get; set; }
    }
}
