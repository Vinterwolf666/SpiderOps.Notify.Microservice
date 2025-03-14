using Xunit;
using Moq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Customer.Notify.Microservice.API.Controllers;
using Customer.Notify.Microservice.APP;
using Customer.Notify.Microservice.Domain;
using Customer.Notify.Microservice.API.Services;
using System.Threading.Tasks;

namespace Customer.Notify.Microservice.Test.Controllers
{


    

    public class NotifyControllerTests
    {
        private readonly Mock<INotifyRepository> _mockRepo;
        private readonly Mock<RabbitMQConsumer> _mockRabbitMQ;
        private readonly NotifyController _controller;

        public NotifyControllerTests()
        {
            _mockRepo = new Mock<INotifyRepository>();
            _mockRabbitMQ = new Mock<RabbitMQConsumer>();
            _controller = new NotifyController(_mockRepo.Object, _mockRabbitMQ.Object);
        }

        [Fact]
        public void AllNotifications_ReturnsOkResult_WithListOfNotifications()
        {
            
            var notifications = new List<Notification> { new Notification { TEXT_MESSAGE = "Test Message", CUSTOMER_ID = 11 } };
            _mockRepo.Setup(repo => repo.AllNotifications()).Returns(notifications);

            
            var result = _controller.AllNotifications();

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Notification>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public void AllNotificationByID_ReturnsOkResult_WithListOfNotifications()
        {
            
            int testId = 11;
            var notifications = new List<Notification> { new Notification { TEXT_MESSAGE = "Test", CUSTOMER_ID = testId } };
            _mockRepo.Setup(repo => repo.AllNotificationsByID(testId)).Returns(notifications);

            
            var result = _controller.AllNotificationByID(testId);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Notification>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task DeleteANotificationByID_ReturnsOkResult_WithStringMessage()
        {
            
            int testId = 1;
            _mockRepo.Setup(repo => repo.DeleteByID(testId)).ReturnsAsync("Deleted Successfully");

            
            var result = await _controller.DeleteANotificationByID(testId);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Deleted Successfully", okResult.Value);
        }

        [Fact]
        public async Task SendAnAccountCreationNotification_ReturnsOkResult_WithStringMessage()
        {
            var notification = new Notification { TEXT_MESSAGE = "Welcome", CUSTOMER_ID = 1 };

            _mockRepo.Setup(repo => repo.SendAccountCreationNotification(notification))
                     .ReturnsAsync("Notification Sent");

            var result = await _controller.sendAnAccountCreationNotification(notification); 

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result); 

            var actualValue = Assert.IsType<string>(okResult.Value); 

            Assert.Equal("Notification Sent", actualValue);
        }




        [Fact]
        public async Task SendRecoveyPassNotification_ReturnsOkResult_WithStringMessage()
        {
           
            string email = "test@mail.com";
            string text_message = "Reset Password";
            int id = 1;
            _mockRepo.Setup(repo => repo.SendRecoveyPassNotification(email, text_message, id))
                     .ReturnsAsync("Recovery Sent");

            
            var result = await _controller.SendRecoveyPassNotification(email, text_message, id);

            
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal("Recovery Sent", okResult.Value);
        }
    }


}
