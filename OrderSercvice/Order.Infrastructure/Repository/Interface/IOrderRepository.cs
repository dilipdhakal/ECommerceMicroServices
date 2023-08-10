using Order.Domain.Entities;
using Shared.Wrapper;

namespace Order.Infrastructure.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<IResponse> InsertOrderAsync(Orders model);
        Task<IResponse<List<Orders>>> GetAllOrderDetailsAsync();
        Task<IResponse<Orders>> GetOrderByIdAsync(int OrderId);
        Task<IResponse> UpdateOrderAsync(Orders model);
        Task<IResponse> DeleteOrderAsync(int OrderId);
    }
}
