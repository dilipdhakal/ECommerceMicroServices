using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Product.Application.Services.Interface;
using Product.Domain.Model.RequestModel;
using Product.Domain.Model.ResponseModel;
using ProductAPI.Controllers;
using Shared.Wrapper;

namespace Prodct.ControlerTest
{
    public class ProductControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IProductService> _ProductServiceMock;
        private readonly ProductController _ProductController;

        public ProductControllerTest()
        {
            _fixture = new Fixture();
            _ProductServiceMock = _fixture.Freeze<Mock<IProductService>>();
            _ProductController = new ProductController(_ProductServiceMock.Object);
        }
        [Fact]
        public async Task GetAllProduct_ShouldReturnOkReposonse_WhenDataFound()
        {
            // Arrange
            var ProductsData = _fixture.Create<List<ProductResponseModel>>(); // Create some test Products data
            _ProductServiceMock.Setup(x => x.GetAllProductDetailsAsync()).ReturnsAsync(ProductsData);

            //Act
            var result = await _ProductController.GetAllProductDetails();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<List<ProductResponseModel>>();
            _ProductServiceMock.Verify(x => x.GetAllProductDetailsAsync(), Times.Once());
        }

        [Fact]
        public async Task GetAllProduct_ShouldReturnNoContentResponse_WhenDataNotFound()
        {
            // Arrange
            var ProductsData = new List<ProductResponseModel>();
            _ProductServiceMock.Setup(x => x.GetAllProductDetailsAsync()).ReturnsAsync(ProductsData);

            //Act
            var result = await _ProductController.GetAllProductDetails();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _ProductServiceMock.Verify(x => x.GetAllProductDetailsAsync(), Times.Once());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOkReposonse_WhenValidInput()
        {
            // Arrange
            var ProductsData = _fixture.Create<ProductResponseModel>(); // Create some test Products data
            int Id = _fixture.Create<int>();
            _ProductServiceMock.Setup(x => x.GetProductByIdAsync(Id)).ReturnsAsync(ProductsData);

            //Act
            var result = await _ProductController.GetProductById(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<ProductResponseModel>();
            _ProductServiceMock.Verify(x => x.GetProductByIdAsync(Id), Times.Once());
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            ProductResponseModel ProductsData = null!; // Create some test Products data
            int Id = _fixture.Create<int>();
            _ProductServiceMock.Setup(x => x.GetProductByIdAsync(Id)).ReturnsAsync(ProductsData);

            //Act
            var result = await _ProductController.GetProductById(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _ProductServiceMock.Verify(x => x.GetProductByIdAsync(Id), Times.Once());

        }

        [Fact]
        public async Task GetProductById_ShouldReturnBadRequest_WhenInputIsEqualOrLessThanZero()
        {
            //Arrange
            var ProductsData = _fixture.Create<ProductResponseModel>();
            int Id = 0;
            _ProductServiceMock.Setup(x => x.GetProductByIdAsync(Id)).ReturnsAsync(ProductsData);

            //Act
            var result = await _ProductController.GetProductById(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _ProductServiceMock.Verify(x => x.GetProductByIdAsync(Id), Times.Never());

        }

        [Fact]
        public async Task CreateProduct_ShouldReturnOkReposonse_WhenValidData()
        {
            // Arrange
            var requestProductsData = _fixture.Create<ProductRequestModel>();

            _ProductServiceMock.Setup(service => service.InsertProductAsync(requestProductsData)).Returns(Response.SuccessAsync("Sucessfully Added"));

            //Act
            var result = await _ProductController.CreateProduct(requestProductsData);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<string>();
            _ProductServiceMock.Verify(x => x.InsertProductAsync(requestProductsData), Times.Once());
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadResponse_WhenInValidInput()
        {
            // Arrange
            var requestProductsData = new ProductRequestModel
            {

            };

            _ProductServiceMock.Setup(x => x.InsertProductAsync(requestProductsData)).Returns(Response.FailAsync("InvaliData"));

            //Act
            var result = await _ProductController.CreateProduct(requestProductsData);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _ProductServiceMock.Verify(x => x.InsertProductAsync(requestProductsData), Times.Never());
        }


        [Fact]
        public async Task UpdateProduct_ShouldReturnBadResponse_WhenInputIsZero()
        {
            // Arrange
            var requestProductsData = _fixture.Create<ProductRequestModel>();
            var realData = new ProductRequestModel
            {
                ProductId = 0,
                ProductName = requestProductsData.ProductName,
                ProductPrice = requestProductsData.ProductPrice,
                Batch = requestProductsData.Batch,
                Status = requestProductsData.Status,
                CreatedBy = requestProductsData.CreatedBy,
                CreatedDate = requestProductsData.CreatedDate

            };
            _ProductServiceMock.Setup(x => x.InsertProductAsync(realData)).Returns(Response.FailAsync("Update Id canot be zero"));

            // Act
            var result = await _ProductController.UpdateProduct(realData);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _ProductServiceMock.Verify(x => x.InsertProductAsync(requestProductsData), Times.Never());
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnBadResponse_WhenInvalidInput()
        {
            // Arrange
            var requestProductsData = _fixture.Create<ProductRequestModel>();
            var realData = new ProductRequestModel
            {
                ProductId = 2,
                ProductName = "",
                ProductPrice = requestProductsData.ProductPrice,
                Batch = requestProductsData.Batch,
                Status = requestProductsData.Status,
                CreatedBy = requestProductsData.CreatedBy,
                CreatedDate = requestProductsData.CreatedDate

            };
            _ProductServiceMock.Setup(x => x.InsertProductAsync(realData)).Returns(Response.FailAsync("Invalid Data"));

            // Act
            var result = await _ProductController.UpdateProduct(realData);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _ProductServiceMock.Verify(x => x.InsertProductAsync(requestProductsData), Times.Never());
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenRecordNotFound()
        {
            // Arrange
            var requestProductsData = _fixture.Create<ProductRequestModel>();
            var responseData = _fixture.Create<ProductRequestModel>();

            _ProductServiceMock.Setup(x => x.UpdateProductAsync(responseData)).Returns(Response.FailAsync("No Content"));

            // Act
            var result = await _ProductController.UpdateProduct(requestProductsData);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _ProductServiceMock.Verify(x => x.InsertProductAsync(requestProductsData), Times.Never());
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOkReposonse_WhenValidData()
        {
            // Arrange
            var requestProductsData = _fixture.Create<ProductRequestModel>();

            _ProductServiceMock.Setup(service => service.UpdateProductAsync(requestProductsData)).Returns(Response.SuccessAsync("Sucessfully Added"));

            //Act
            var result = await _ProductController.UpdateProduct(requestProductsData);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<string>();
            _ProductServiceMock.Verify(x => x.UpdateProductAsync(requestProductsData), Times.Once());
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnBadResponse_WhenInputIsZero()
        {
            // Arrange
            int Id = 0;

            _ProductServiceMock.Setup(x => x.DeleteProductAsync(Id)).Returns(Response.FailAsync("Deleted Id canot be zero"));

            // Act
            var result = await _ProductController.DeleteProduct(Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _ProductServiceMock.Verify(x => x.DeleteProductAsync(Id), Times.Never());
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenRecordNotFound()
        {
            int Id = _fixture.Create<int>();

            _ProductServiceMock.Setup(x => x.DeleteProductAsync(Id)).Returns(Response.FailAsync("Deleted Id canot be zero"));

            // Act
            var result = await _ProductController.DeleteProduct(Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _ProductServiceMock.Verify(x => x.DeleteProductAsync(Id), Times.Once());
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnOkReposonse_WhenValidData()
        {
            // Arrange
            int Id = _fixture.Create<int>();


            _ProductServiceMock.Setup(x => x.DeleteProductAsync(Id)).Returns(Response.SuccessAsync("Sucessfully Deleted"));

            //Act
            var result = await _ProductController.DeleteProduct(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<string>();
            _ProductServiceMock.Verify(x => x.DeleteProductAsync(Id), Times.Once());
        }

    }
}