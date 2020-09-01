using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sol_Demo.Models
{
    public class ProductModel
    {
        public decimal? ProductId { get; set; }

        public String Name { get; set; }

        public decimal? UnitPrice { get; set; }

        public byte[] Version { get; set; }
    }
}