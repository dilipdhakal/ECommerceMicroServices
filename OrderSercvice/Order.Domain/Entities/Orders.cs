using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities
{
    [Table("tblOrder",Schema ="Orders")]
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public int OrderBy { get; set; }
        public DateTime OrderedDate { get; set; }
        public DateTime? OrderedModifiedDate { get; set; }
        public int Status { get; set; }
    }
}
