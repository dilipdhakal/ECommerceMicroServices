using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.ResponseModel
{
    public class SystemResponse
    {
        public bool Status { get; set; }
        public string? Message { get; set; }
        //public string Data { get; set; }
        //public string Extras { get; set; }
        //public string ExtrasSecond { get; set; }
    }
}
