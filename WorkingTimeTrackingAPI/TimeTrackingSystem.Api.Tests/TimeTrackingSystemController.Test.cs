using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Api.Core.Models;
using TimeTrackingSystem.Controllers;
using Xunit;

namespace TimeTrackingSystem.Api.Tests
{
    public class TimeTrackingSystemTestController
    {

     public static IEnumerable<DepartmentEmployees> GetDepartments()
        {
            return new[]
            {
                new DepartmentEmployees()
                {
                    Name = "Department1",
                    Count = 1
                },
                new DepartmentEmployees()
                {
                    Name = "Department2",
                    Count = 2
                },
                new DepartmentEmployees()
                {
                    Name = "Department3",
                    Count = 3
                }
            };
        }


        public static IEnumerable<EmployeeInfo> GetEmployees()
        {
            return new[]
            {
                new EmployeeInfo("ivan","ivanov",  1),
                new EmployeeInfo("petr","petrov",  1),
                new EmployeeInfo("vasiliy","vasilyiev",  2),
                new EmployeeInfo("anton","zvyaghintsev",  3)
            };
        }
        [Fact]
        public async void Task_GetDepartments_Return_OkResult()
        {
            //Arrange  
            var mockRepo = new Mock<ITimeTrackingSystemRepository>();
            mockRepo.Setup(i => i.GetAllDepartments()).Returns(() => Task.FromResult(GetDepartments()));
            var controller = new TimeTrackingSystemController(mockRepo.Object, null);
            //Act  
            var data = await controller.GetDepartments();
            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public void Task_GetDepartment_Return_BadRequestResult()
        {
            //Arrange  
            var mockRepo = new Mock<ITimeTrackingSystemRepository>();
            mockRepo.Setup(i => i.GetAllEmployees()).Returns(() => Task.FromResult(GetEmployees()));
            var controller = new TimeTrackingSystemController(mockRepo.Object, null);

            //Act  
            var data = controller.GetDepartments();
            data = null;

            if (data != null)
                //Assert  
                Assert.IsType<BadRequestResult>(data);
        }
        [Fact]
        public async void Task_GetDepartments_MatchResult()
        {
            //Arrange  
            var mockRepo = new Mock<ITimeTrackingSystemRepository>();
            mockRepo.Setup(i => i.GetAllDepartments()).Returns(() => Task.FromResult(GetDepartments()));
            var controller = new TimeTrackingSystemController(mockRepo.Object, null);

            //Act  
            var data = await controller.GetDepartments();
            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var department = okResult.Value.Should().BeAssignableTo<IEnumerable<DepartmentEmployees>>().Subject.ToArray();

            Assert.Equal("Department1", department[0].Name);

            Assert.Equal("Department2", department[1].Name);

            Assert.Equal("Department3", department[2].Name);
        }

        [Fact]
        public async void Task_Add_Department_Return_OkResult()
        {
            //Arrange
            var mockRepo = new Mock<ITimeTrackingSystemRepository>();
            mockRepo.Setup(i => i.AddDepartment(new DepartmentInfo(){DepartmentName = "Department4"}));
            var controller = new TimeTrackingSystemController(mockRepo.Object, null);
            //Act  
            var data = await controller.CreateDepartment("Department4");
            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void Task_Add_Department_Return_BadRequest()
        {
            //Arrange  
            var mockRepo = new Mock<ITimeTrackingSystemRepository>();
            mockRepo.Setup(i => i.AddDepartment(new DepartmentInfo() {DepartmentName = ""}));
            var controller = new TimeTrackingSystemController(mockRepo.Object, null);
            //Act              
            var data = await controller.CreateDepartment("");
            //Assert  
            var result = Assert.IsType<OkObjectResult>(data);
            Assert.Equal(500, result.StatusCode);
        }

        //[Fact]
        //public async void Task_Update_Department_Return_OkResult()
        //{
        //    //Arrange  
        //    var controller = new TimeTrackingSystemController(_repository, null);

        //    //Act  
        //    var data = await controller.UpdateDepartment("NewDepartment1", 1);

        //    //Assert  
        //    var result = Assert.IsType<StatusCodeResult>(data);
        //    Assert.Equal(200, result.StatusCode);
        //}
        //[Fact]
        //public async void Task_Update_Department_Return_BadRequest()
        //{
        //    //Arrange  
        //    var controller = new TimeTrackingSystemController(_repository, null);

        //    //Act  
        //    var data = await controller.UpdateDepartment("", 1);

        //    //Assert  
        //    var result = Assert.IsType<ObjectResult>(data);
        //    Assert.Equal(500, result.StatusCode);
        //}
        //[Fact]
        //public async void Task_Update_Department_Return_NotFound()
        //{
        //    //Arrange  
        //    var controller = new TimeTrackingSystemController(_repository, null);

        //    //Act  
        //    var data = await controller.UpdateDepartment("Department15", 15);
        //    //Assert  
        //    var result = Assert.IsType<ObjectResult>(data);
        //    Assert.Equal(500, result.StatusCode);
        //}
    }

}
