using AutoMapper;
using Prodcut.Domain.Entities;
using Product.Domain.Model.ResponseModel;
using Product.Application.Services.Interface;
using Product.Infrastructure.Repository.Interface;
using Shared.Wrapper;
using Product.Domain.Model.RequestModel;

namespace Product.Application.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productService;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository ProductRepo, IMapper map)
        {
            _productService = ProductRepo;
            _mapper = map;
        }
        public async Task<IResponse> DeleteProductAsync(int ProductId)
        {
            return await _productService.DeleteProductAsync(ProductId);

        }

        public async Task<List<ProductResponseModel>> GetAllProductDetailsAsync()
        {
            var data = await _productService.GetAllProductDetailsAsync();
            var mapdata = _mapper.Map<List<ProductResponseModel>>(data.Data);
            return mapdata;
        }

        public async Task<ProductResponseModel> GetProductByIdAsync(int ProductId)
        {
            var data = await _productService.GetProductByIdAsync(ProductId);
            var mapdata = _mapper.Map<ProductResponseModel>(data.Data);
            return mapdata;
        }

        public async Task<IResponse> InsertProductAsync(ProductRequestModel model)
        {
            var mapdata = _mapper.Map<Products>(model);
            return await _productService.InsertProductAsync(mapdata);
        }

        public async Task<IResponse> UpdateProductAsync(ProductRequestModel model)
        {
            var mapdata = _mapper.Map<Products>(model);
            return await _productService.UpdateProductAsync(mapdata);
        }
    }
}
