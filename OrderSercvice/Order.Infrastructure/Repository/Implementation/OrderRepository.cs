using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Domain.Entities;
using Order.Infrastructure.Repository.Interface;
using Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _db;
        public readonly ILogger<OrderRepository> _log;
        public OrderRepository(OrderDbContext dbContext, ILogger<OrderRepository> log)
        {
            _db = dbContext;
            _log = log;
        }
        public async Task<IResponse> DeleteOrderAsync(int OrderId)
        {
            try
            {
                var existingData = _db.orders.Where(x => x.OrderId == OrderId).FirstOrDefault();
                if (existingData != null)
                {
                    _db.orders.Remove(existingData);
                    _db.SaveChanges();
                    _log.LogInformation("Deleted Prodcut Id:", OrderId.ToString());
                    return await Response.SuccessAsync("Order Deleted Successfully");
                }
                else
                {
                    _log.LogInformation("Delete Invalid OrderId :", OrderId.ToString());
                    return await Response.FailAsync("Invalid OrderId");
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.InnerException + ":", OrderId.ToString());
                return await Response.FailAsync(ex.Message);
            }
        }

        public async Task<IResponse<List<Orders>>> GetAllOrderDetailsAsync()
        {
            var ordersDetails = await _db.orders.ToListAsync();
            _log.LogInformation("Order All Details", ordersDetails);
            return await Response<List<Orders>>.SuccessAsync(ordersDetails);
        }

        public async Task<IResponse<Orders>> GetOrderByIdAsync(int OrderId)
        {
            var existingData = await _db.orders.Where(x => x.OrderId == OrderId).FirstOrDefaultAsync();
            _log.LogInformation("Order All Details", existingData);
            return await Response<Orders>.SuccessAsync(existingData!);
        }

        public async Task<IResponse> InsertOrderAsync(Orders model)
        {
            try
            {
                await _db.AddAsync(model);
                await _db.SaveChangesAsync();
                return await Response.SuccessAsync("Order Created Successfully");
            }
            catch (Exception ex)
            {
                _log.LogInformation("Insert order", model);
                return await Response.FailAsync(ex.Message);
            }
        }

        public async Task<IResponse> UpdateOrderAsync(Orders model)
        {

            try
            {
                var existData = await _db.orders.SingleOrDefaultAsync(x => x.OrderId == model.OrderId);

                if (existData == null)
                {
                    return await Response.FailAsync("Order does not Exists");
                }
                else
                {
                    existData.OrderId = model.OrderId;
                    existData.ProductId = model.ProductId;
                    existData.Quantity = model.Quantity;
                    existData.Remarks = model.Remarks;
                    existData.OrderBy = model.OrderBy;
                    existData.OrderedDate = model.OrderedDate;
                    existData.OrderedModifiedDate = model.OrderedModifiedDate;
                    existData.Status = model.Status;
                    _db.Update(existData);
                    await _db.SaveChangesAsync();
                    return await Response.SuccessAsync("Order Updated Successfully");
                }

            }
            catch (Exception ex)
            {
                _log.LogInformation("Invalid Data For Update" + ":", model.OrderId.ToString());
                return await Response.FailAsync(ex.Message);
            }


        }
    }
}
