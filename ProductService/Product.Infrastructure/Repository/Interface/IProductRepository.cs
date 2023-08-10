using Prodcut.Domain.Entities;
using Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repository.Interface
{
    public interface IProductRepository
    {
        Task<IResponse> InsertProductAsync(Products model);
        Task<IResponse<List<Products>>> GetAllProductDetailsAsync();
        Task<IResponse<Products>> GetProductByIdAsync(int ProductId);
        Task<IResponse> UpdateProductAsync(Products model);
        Task<IResponse> DeleteProductAsync(int ProductId);
    }
}
