using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TimeTrackingSystem.Api.Core;
using TimeTrackingSystem.Controllers;
using TimeTrackingSystem.Data.Access.Context;
using TimeTrackingSystem.Data.Access.DAL;
using TimeTrackingSystem.Data.Model;
using TimeTrackingSystem.Models;
using Xunit;

namespace TimeTrackingSystem.Api.Tests
{
    public class MockDataDBInitializer
    {
        public void Seed(TimeTrackingSystemDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Departments.AddRange(new[]
            {
                new Department()
                {
                    DepartmentName = "Department1",
                    DepartmentId = 1
                },
                new Department()
                {
                    DepartmentName = "Department2",
                    DepartmentId = 2
                },
                new Department()
                {
                    DepartmentName = "Department3",
                    DepartmentId = 3
                }
            });

            context.SaveChanges();
            context.Employees.AddRange(new[]{
                new Employee()
                {
                    FirstName = "ivan",
                    LastName = "ivanov",
                    Department = context.Departments.First(t=>t.DepartmentId==1)
                },
                new Employee()
                {
                    FirstName = "petr",
                    LastName = "petrov",
                    Department = context.Departments.First(t=>t.DepartmentId==1)
                },
                new Employee()
                {
                    FirstName = "vasiliy",
                    LastName = "vasilyiev",
                    Department = context.Departments.First(t=>t.DepartmentId==2)
                },
                new Employee()
                {
                    FirstName = "anton",
                    LastName = "zvyaghintsev",
                    Department = context.Departments.First(t=>t.DepartmentId==3)
                }
            });
            context.SaveChanges();
        }
    }

    public class TimeTrackingSystemTestController
    {
        private ITimeTrackingSystemRepository _repository;
        public static DbContextOptions<TimeTrackingSystemDbContext> dbContextOptions { get; }
        public static string connectionString = "Filename=local.db";

        static TimeTrackingSystemTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<TimeTrackingSystemDbContext>().UseSqlServer(connectionString).Options;
        }
        public TimeTrackingSystemTestController()
        {
            var context = new TimeTrackingSystemDbContext();
            var db = new MockDataDBInitializer();
            db.Seed(context);
            _repository = new TimeTrackingSystemRepository(context, new NullLoggerFactory());
        }

        [Fact]
        public async void Task_GetDepartments_Return_OkResult()
        {
            //Arrange  
            var controller = new TimeTrackingSystemController(_repository, null);
            //Act  
            var data = await controller.GetDepartments();
            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public void Task_GetDepartment_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new TimeTrackingSystemController(_repository, null);

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
            var controller = new TimeTrackingSystemController(_repository, null);

            //Act  
            var data = await controller.GetDepartments();
            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var department = okResult.Value.Should().BeAssignableTo<IEnumerable<DepartmentInfo>>().Subject.ToArray();

            Assert.Equal("Department1", department[0].DepartmentName);

            Assert.Equal("Department2", department[1].DepartmentName);

            Assert.Equal("Department3", department[2].DepartmentName);
        }

        [Fact]
        public async void Task_Add_Department_Return_OkResult()
        {
            //Arrange  
            var controller = new TimeTrackingSystemController(_repository, null);
            //Act  
            var data = await controller.CreateDepartment("Department4");
            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void Task_Add_Department_Return_BadRequest()
        {
            //Arrange  
            var controller = new TimeTrackingSystemController(_repository, null);
            //Act              
            var data = await controller.CreateDepartment("");
            //Assert  
            var result = Assert.IsType<ObjectResult>(data);
            Assert.Equal(500, result.StatusCode);
        }

        [Fact]
        public async void Task_Update_Department_Return_OkResult()
        {
            //Arrange  
            var controller = new TimeTrackingSystemController(_repository, null);

            //Act  
            var data = await controller.UpdateDepartment("NewDepartment1", 1);

            //Assert  
            var result = Assert.IsType<StatusCodeResult>(data);
            Assert.Equal(200, result.StatusCode);
        }
        [Fact]
        public async void Task_Update_Department_Return_BadRequest()
        {
            //Arrange  
            var controller = new TimeTrackingSystemController(_repository, null);

            //Act  
            var data = await controller.UpdateDepartment("",1);

            //Assert  
            var result = Assert.IsType<ObjectResult>(data);
            Assert.Equal(500, result.StatusCode);
        }
        [Fact]
        public async void Task_Update_Department_Return_NotFound()
        {
            //Arrange  
            var controller = new TimeTrackingSystemController(_repository, null);

            //Act  
            var data = await controller.UpdateDepartment("Department15", 15);
            //Assert  
            var result = Assert.IsType<ObjectResult>(data);
            Assert.Equal(500, result.StatusCode);
        }
    }

}
