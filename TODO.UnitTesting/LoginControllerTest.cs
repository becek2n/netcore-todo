using Microsoft.Extensions.Logging;
using Moq;
using System;
using TODO.API.Controllers;
using Xunit;

namespace TODO.UnitTesting
{
    public class LoginControllerTest
    {
        private readonly Mock<ILogger<TaskController>> _logger;
        public LoginControllerTest() { 
            
        }
        [Fact]
        public void Task_Login_Return_OkResult()
        {
            //arrange 
            //var controller = new UserController();


        }
    }
}
