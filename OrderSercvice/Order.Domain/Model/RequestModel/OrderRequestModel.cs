using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Model.ResponseModel
{
    public class OrderRequestModel
    {
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        public string Remarks { get; set; } = string.Empty;
        [Required]
        public int OrderBy { get; set; }
        public DateTime OrderedDate { get; set; }
        public DateTime? OrderedModifiedDate { get; set; }
        public int Status { get; set; }
    }
}
