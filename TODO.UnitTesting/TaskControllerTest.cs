using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Security.Claims;
using TODO.API.Controllers;
using TODO.DTO;
using TODO.Interfaces;
using Xunit;

namespace TODO.UnitTesting
{
    public class TaskControllerTest
    {
        private readonly Mock<ILogger<TaskController>> _logger;
        private readonly Mock<ITask> _task;
        public TaskControllerTest() {
            _logger = new Mock<ILogger<TaskController>>();
            _task = new Mock<ITask>();
        }

        [Fact]
        public async void Task_GetId_Return_OkResult()
        {
            //arrange 
            var dtoTask = new TaskDTO() { Id = 1, Name = "test 1" };
            _task.Setup(x => x.GetId(1))
                .ReturnsAsync(new TaskDTO());

            //_task.Setup(s => s.GetId(It.IsAny<int>()))
            //    .ReturnsAsync((int id) => new TaskDTO { 
            //        Id = id,
            //        Name = "task 2"
            //    });
                //.ReturnsAsync((int id) => new ResultModel<TaskDTO>());

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(type: "Id", value: "1"),
                        new Claim(ClaimTypes.Name, "test"),
                    })
                })
            };

            var controller = new TaskController(_logger.Object, _task.Object) {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.GetById(1);

            //assert
            var okres = result as OkObjectResult;
            var okActionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskDTO>(okActionResult.Value);
            Assert.Equal(1, returnValue.Id);

        }

        [Fact]
        public async void Task_GetId_Return_Unauthorize()
        {
            //arrange 
            var dtoTask = new TaskDTO() { Id = 1, Name = "test 1" };
            _task.Setup(x => x.GetId(1))
                .ReturnsAsync(dtoTask);

            var httpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new[]
                {
                    new ClaimsIdentity(new[]
                    {
                        new Claim(type: "Id", value: "1"),
                        new Claim(ClaimTypes.Name, "test"),
                    })
                })
            };

            var controller = new TaskController(_logger.Object, _task.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = httpContext }
            };

            var result = await controller.GetById(2);

            //assert
            Assert.IsType<UnauthorizedResult>(result);

        }
    }
}
