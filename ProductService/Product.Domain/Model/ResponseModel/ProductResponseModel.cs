using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Model.ResponseModel
{
    public class ProductResponseModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public string Batch { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
