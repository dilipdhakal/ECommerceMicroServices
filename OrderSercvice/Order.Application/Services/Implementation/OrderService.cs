using AutoMapper;
using Order.Domain.Model.ResponseModel;
using Order.Application.Services.Interface;
using Order.Domain.Entities;
using Order.Infrastructure.Repository.Interface;
using Shared.Wrapper;

namespace Order.Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepo, IMapper map)
        {
            _orderRepo = orderRepo;
            _mapper = map;
        }
        public async Task<IResponse> DeleteOrderAsync(int OrderId)
        {
            return await _orderRepo.DeleteOrderAsync(OrderId);

        }

        public async Task<List<OrderResponseModel>> GetAllOrderDetailsAsync()
        {
            var data = await _orderRepo.GetAllOrderDetailsAsync();
            var mapdata = _mapper.Map<List<OrderResponseModel>>(data.Data);
            return mapdata;
        }

        public async Task<OrderResponseModel> GetOrderByIdAsync(int OrderId)
        {
            var data = await _orderRepo.GetOrderByIdAsync(OrderId);
            var mapdata = _mapper.Map<OrderResponseModel>(data.Data);
            return mapdata;
        }

        public async Task<IResponse> InsertOrderAsync(OrderRequestModel model)
        {
           var mapdata = _mapper.Map<Orders>(model);
            return await _orderRepo.InsertOrderAsync(mapdata);
        }

        public async Task<IResponse> UpdateOrderAsync(OrderRequestModel model)
        {
            var mapdata = _mapper.Map<Orders>(model);
            return await _orderRepo.UpdateOrderAsync(mapdata);
        }
    }
}
