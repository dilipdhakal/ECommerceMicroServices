using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Domain.Model.ResponseModel;
using Order.Application.Services.Interface;
using Order.Domain.Entities;

namespace OrderAPI.Controllers
{
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _proService;
        public OrderController(IOrderService proService)
        {
            _proService = proService;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("CreateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestModel model)
        {
            try
            {
                if (model.ProductId == 0)
                {
                    return BadRequest();
                }
                var isSuccess = await _proService.InsertOrderAsync(model);
                if (isSuccess.Succeeded)
                {
                    return Ok(isSuccess.Messages);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderById")]
        public async Task<IActionResult> GetOrderById(int OrderId)
        {
            try
            {
                if (OrderId <= 0)
                {
                    return BadRequest();
                }
                var orderData = await _proService.GetOrderByIdAsync(OrderId);

                if (orderData != null)
                {
                    return Ok(orderData);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrderDetails")]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            try
            {
                var orderList = await _proService.GetAllOrderDetailsAsync();
                if (orderList.Count() > 0)
                {

                    return Ok(orderList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(OrderRequestModel model)
        {
            try
            {
              
                if (model.OrderId <= 0 || model.ProductId == 0 || model.OrderBy == 0 || model.Quantity == 0)
                {
                    return BadRequest();
                }
                var isSuccess = await _proService.UpdateOrderAsync(model);
                if (isSuccess.Succeeded)
                {
                    return Ok(isSuccess.Messages);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int OrderId)
        {
            try
            {
                if (OrderId <= 0)
                {
                    return BadRequest();
                }

                var isSuccess = await _proService.DeleteOrderAsync(OrderId);
                if (isSuccess.Succeeded)
                {
                    return Ok(isSuccess.Messages);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}

