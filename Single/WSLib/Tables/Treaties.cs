using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSLib.Tables
{
    public class Treaties
    {
        public Treaties()
        {
            Id = -1;
        }
        public int Id { get; set; }
        public int Number { get; set; } 
        public DateTime BuyDate { get; set; }
        public string ClientFullName { get; set; }
        public string ModelName { get; set; }
        public string UserFullName { get; set; }
    }
}
