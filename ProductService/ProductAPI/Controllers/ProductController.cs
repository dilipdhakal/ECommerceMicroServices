using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.Domain.Model.ResponseModel;
using Product.Application.Services.Interface;
using Product.Domain.Model.RequestModel;

namespace ProductAPI.Controllers
{
    public class ProductController : BaseAPIController
    {
        private readonly IProductService _proService;
        public ProductController(IProductService proService)
        {
            _proService = proService;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequestModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.ProductName))
                {
                    return BadRequest();
                }
                var isSuccess = await _proService.InsertProductAsync(model);
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
        [HttpGet("GetProductById")]
        public async Task<IActionResult> GetProductById(int ProductId)
        {
            try
            {
                if (ProductId <= 0)
                {
                    return BadRequest();
                }
                var productData = await _proService.GetProductByIdAsync(ProductId);
                if (productData != null)
                {
                    return Ok(productData);
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
        [HttpGet("GetAllProductDetails")]
        public async Task<IActionResult> GetAllProductDetails()
        {
            try
            {
                var productList = await _proService.GetAllProductDetailsAsync();
                if (productList.Count() > 0)
                {
                    return Ok(productList);
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
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(ProductRequestModel model)
        {
            try
            {
                if (model.ProductId <= 0 || model.ProductName == "")
                {
                    return BadRequest();
                }

                if (model == null)
                {
                    return BadRequest();
                }
                var isSuccess = await _proService.UpdateProductAsync(model);
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
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int ProductId)
        {
            try
            {
                if (ProductId <= 0)
                {
                    return BadRequest();
                }

                var isSuccess = await _proService.DeleteProductAsync(ProductId);
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
