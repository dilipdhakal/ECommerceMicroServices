using Prodcut.Domain.Entities;
using Product.Domain.Model.RequestModel;
using Product.Domain.Model.ResponseModel;
using Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application.Services.Interface
{
    public interface IProductService
    {
        Task<IResponse> InsertProductAsync(ProductRequestModel model);
        Task<List<ProductResponseModel>> GetAllProductDetailsAsync();
        Task<ProductResponseModel> GetProductByIdAsync(int ProductId);
        Task<IResponse> UpdateProductAsync(ProductRequestModel model);
        Task<IResponse> DeleteProductAsync(int ProductId);
    }
}
