using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Order.Application.Services.Interface;
using Order.Domain.Model.ResponseModel;
using OrderAPI.Controllers;
using Shared.Wrapper;

namespace Order.Test.Controller
{
    public class OrderControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrderController _orderController;

        public OrderControllerTest()
        {
            _fixture = new Fixture();
            _orderServiceMock = _fixture.Freeze<Mock<IOrderService>>();
            _orderController = new OrderController(_orderServiceMock.Object);
        }
        [Fact]
        public async Task GetAllOrder_ShouldReturnOkReposonse_WhenDataFound()
        {
            // Arrange
            var ordersData = _fixture.Create<List<OrderResponseModel>>(); // Create some test orders data
            _orderServiceMock.Setup(x => x.GetAllOrderDetailsAsync()).ReturnsAsync(ordersData);

            //Act
            var result = await _orderController.GetAllOrderDetails();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<List<OrderResponseModel>>();
            _orderServiceMock.Verify(x => x.GetAllOrderDetailsAsync(), Times.Once());
        }

        [Fact]
        public async Task GetAllOrder_ShouldReturnNoContentResponse_WhenDataNotFound()
        {
            // Arrange
            var ordersData = new List<OrderResponseModel>();
            _orderServiceMock.Setup(x => x.GetAllOrderDetailsAsync()).ReturnsAsync(ordersData);

            //Act
            var result = await _orderController.GetAllOrderDetails();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _orderServiceMock.Verify(x => x.GetAllOrderDetailsAsync(), Times.Once());
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOkReposonse_WhenValidInput()
        {
            // Arrange
            var ordersData = _fixture.Create<OrderResponseModel>(); // Create some test orders data
            int Id = _fixture.Create<int>();
            _orderServiceMock.Setup(x => x.GetOrderByIdAsync(Id)).ReturnsAsync(ordersData);

            //Act
            var result = await _orderController.GetOrderById(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<OrderResponseModel>();
            _orderServiceMock.Verify(x => x.GetOrderByIdAsync(Id), Times.Once());
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnNotFound_WhenDataNotFound()
        {
            //Arrange
            OrderResponseModel ordersData = null!; // Create some test orders data
            int Id = _fixture.Create<int>();
            _orderServiceMock.Setup(x => x.GetOrderByIdAsync(Id)).ReturnsAsync(ordersData);

            //Act
            var result = await _orderController.GetOrderById(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _orderServiceMock.Verify(x => x.GetOrderByIdAsync(Id), Times.Once());

        }

        [Fact]
        public async Task GetOrderById_ShouldReturnBadRequest_WhenInputIsEqualOrLessThanZero()
        {
            //Arrange
            var ordersData = _fixture.Create<OrderResponseModel>();
            int Id = 0;
            _orderServiceMock.Setup(x => x.GetOrderByIdAsync(Id)).ReturnsAsync(ordersData);

            //Act
            var result = await _orderController.GetOrderById(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _orderServiceMock.Verify(x => x.GetOrderByIdAsync(Id), Times.Never());

        }

        [Fact]
        public async Task CreateOrder_ShouldReturnOkReposonse_WhenValidData()
        {
            // Arrange
            var requestOrdersData = _fixture.Create<OrderRequestModel>();

            _orderServiceMock.Setup(service => service.InsertOrderAsync(requestOrdersData)).Returns(Response.SuccessAsync("Sucessfully Added"));

            //Act
            var result = await _orderController.CreateOrder(requestOrdersData);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<string>();
            _orderServiceMock.Verify(x => x.InsertOrderAsync(requestOrdersData), Times.Once());
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnBadReponse_WhenInValidData()
        {
            // Arrange
            var requestOrdersData = new OrderRequestModel
            {

            };

            _orderServiceMock.Setup(x => x.InsertOrderAsync(requestOrdersData)).Returns(Response.FailAsync("InvaliData"));

            //Act
            var result = await _orderController.CreateOrder(requestOrdersData);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _orderServiceMock.Verify(x => x.InsertOrderAsync(requestOrdersData), Times.Never());
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadResponse_WhenInputIsZero()
        {
            // Arrange
            var requestOrdersData = _fixture.Create<OrderRequestModel>();
            var realData = new OrderRequestModel
            {
                OrderId = 0,
                ProductId = requestOrdersData.ProductId,
                Quantity = requestOrdersData.Quantity,
                Remarks = requestOrdersData.Remarks,
                OrderBy = requestOrdersData.OrderBy,
                OrderedModifiedDate = requestOrdersData.OrderedModifiedDate,
                Status = requestOrdersData.Status,

            };
            _orderServiceMock.Setup(x => x.InsertOrderAsync(realData)).Returns(Response.FailAsync("Update Id canot be zero"));

            // Act
            var result = await _orderController.UpdateOrder(realData);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _orderServiceMock.Verify(x => x.InsertOrderAsync(requestOrdersData), Times.Never());
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnBadResponse_WhenInvalidInput()
        {
            // Arrange
            var requestOrdersData = _fixture.Create<OrderRequestModel>();
            var realData = new OrderRequestModel
            {
                OrderId = 2,
                ProductId = 0,
                Quantity = requestOrdersData.Quantity,
                Remarks = requestOrdersData.Remarks,
                OrderBy = 0,
                OrderedModifiedDate = requestOrdersData.OrderedModifiedDate,
                Status = requestOrdersData.Status,

            };
            _orderServiceMock.Setup(x => x.InsertOrderAsync(realData)).Returns(Response.FailAsync("Invalid Data"));

            // Act
            var result = await _orderController.UpdateOrder(realData);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _orderServiceMock.Verify(x => x.InsertOrderAsync(requestOrdersData), Times.Never());
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnNotFound_WhenRecordNotFound()
        {
            // Arrange
            var requestOrdersData = _fixture.Create<OrderRequestModel>();
            var responseData = _fixture.Create<OrderRequestModel>();

            _orderServiceMock.Setup(x => x.UpdateOrderAsync(responseData)).Returns(Response.FailAsync("No Content"));

            // Act
            var result = await _orderController.UpdateOrder(requestOrdersData);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _orderServiceMock.Verify(x => x.InsertOrderAsync(requestOrdersData), Times.Never());
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnOkReposonse_WhenValidData()
        {
            // Arrange
            var requestOrdersData = _fixture.Create<OrderRequestModel>();

            _orderServiceMock.Setup(service => service.UpdateOrderAsync(requestOrdersData)).Returns(Response.SuccessAsync("Sucessfully Added"));

            //Act
            var result = await _orderController.UpdateOrder(requestOrdersData);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<string>();
            _orderServiceMock.Verify(x => x.UpdateOrderAsync(requestOrdersData), Times.Once());
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnBadResponse_WhenInputIsZero()
        {
            // Arrange
            int Id = 0;

            _orderServiceMock.Setup(x => x.DeleteOrderAsync(Id)).Returns(Response.FailAsync("Deleted Id canot be zero"));

            // Act
            var result = await _orderController.DeleteOrder(Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
            _orderServiceMock.Verify(x => x.DeleteOrderAsync(Id), Times.Never());
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNotFound_WhenRecordNotFound()
        {
            int Id = _fixture.Create<int>();

            _orderServiceMock.Setup(x => x.DeleteOrderAsync(Id)).Returns(Response.FailAsync("Deleted Id canot be zero"));

            // Act
            var result = await _orderController.DeleteOrder(Id);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundResult>();
            _orderServiceMock.Verify(x => x.DeleteOrderAsync(Id), Times.Once());
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnOkReposonse_WhenValidData()
        {
            // Arrange
            int Id = _fixture.Create<int>();


            _orderServiceMock.Setup(x => x.DeleteOrderAsync(Id)).Returns(Response.SuccessAsync("Sucessfully Deleted"));

            //Act
            var result = await _orderController.DeleteOrder(Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            var okResult = result.As<OkObjectResult>(); // Cast the result to OkObjectResult
            okResult.Value.Should().NotBeNull(); // Check if the value within the OkObjectResult is not null
            okResult.Value.Should().BeAssignableTo<string>();
            _orderServiceMock.Verify(x => x.DeleteOrderAsync(Id), Times.Once());
        }

    }
}