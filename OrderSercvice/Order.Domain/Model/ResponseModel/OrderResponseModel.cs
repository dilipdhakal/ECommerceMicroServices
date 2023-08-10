using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Model.ResponseModel
{
    public class OrderResponseModel
    {
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
