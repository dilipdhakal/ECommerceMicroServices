using Order.Domain.Model.ResponseModel;
using Order.Domain.Entities;
using Shared.Wrapper;

namespace Order.Application.Services.Interface
{
    public interface IOrderService
    {
        Task<IResponse> InsertOrderAsync(OrderRequestModel model);
        Task<List<OrderResponseModel>> GetAllOrderDetailsAsync();
        Task<OrderResponseModel> GetOrderByIdAsync(int OrderId);
        Task<IResponse> UpdateOrderAsync(OrderRequestModel model);
        Task<IResponse> DeleteOrderAsync(int OrderId);
    }
}
