using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Prodcut.Domain.Entities;
using Product.Infrastructure.Repository.Interface;
using Shared.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _db;
        public readonly ILogger<ProductRepository> _log;
        public ProductRepository(ProductDbContext dbCxt, ILogger<ProductRepository> log)
        {
            _db = dbCxt;
            _log = log;
        }
        public async Task<IResponse> DeleteProductAsync(int ProductId)
        {
            try
            {
                var existingData = _db.products.Where(x => x.ProductId == ProductId).FirstOrDefault();
                if (existingData != null)
                {
                    _db.products.Remove(existingData);
                    _db.SaveChanges();
                    _log.LogInformation("Deleted Prodcut Id:", ProductId.ToString());
                    return await Response.SuccessAsync("Product Deleted Successfully");
                }
                else
                {
                    _log.LogInformation("Delete Invalid ProductId :", ProductId.ToString());
                    return await Response.FailAsync("Invalid ProductId");
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.InnerException + ":", ProductId.ToString());
                return await Response.FailAsync(ex.Message);
            }

        }

        public async Task<IResponse<List<Products>>> GetAllProductDetailsAsync()
        {
            var productDetails = await _db.products.ToListAsync();
            _log.LogInformation("Product All Details", productDetails);
            return await Response<List<Products>>.SuccessAsync(productDetails);

        }

        public async Task<IResponse<Products>> GetProductByIdAsync(int ProductId)
        {
            var existingData = await _db.products.Where(x => x.ProductId == ProductId).FirstOrDefaultAsync();
            _log.LogInformation("Product All Details", existingData);
            return await Response<Products>.SuccessAsync(existingData);
        }

        public async Task<IResponse> InsertProductAsync(Products model)
        {
            try
            {
                _db.products.Add(model);
                _db.SaveChanges();
                _log.LogInformation("insert Product", model);
                return await Response.SuccessAsync("Product Inserted Successfully");
            }
            catch (Exception ex)
            {
                _log.LogInformation("Insert Product", model);
                return await Response.FailAsync(ex.Message);
            }

        }

        public async Task<IResponse> UpdateProductAsync(Products model)
        {
            try
            {
                var existData = await _db.products.SingleOrDefaultAsync(x => x.ProductId == model.ProductId);

                if (existData == null)
                {
                    return await Response.FailAsync("Product does not Exists");
                }
                else
                {
                    existData.ProductId = model.ProductId;
                    existData.ProductName = model.ProductName;
                    existData.ProductPrice = model.ProductPrice;
                    existData.Batch = model.Batch;
                    existData.Status = model.Status;
                    existData.CreatedBy = model.CreatedBy;
                    existData.CreatedDate = Convert.ToDateTime(model.CreatedDate).ToUniversalTime();
                    _db.Update(existData);
                    await _db.SaveChangesAsync();
                    return await Response.SuccessAsync("Product Updated Successfully");
                }

            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.InnerException + ":", model.ProductId.ToString());
                return await Response.FailAsync(ex.Message);
            }

        }
    }
}
