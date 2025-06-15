using Moq;
using HRManagement.API.Controllers;
using HRManagement.API.Repositories.Interfaces;
using HRManagement.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using HRManagement.API.DTOs;

namespace HRManagement.Tests
{
    [TestClass]
    public class EmployeesControllerTests
    {
        private EmployeesController _controller = null!;
        private Mock<IEmployeeRepository> _repoMock = null!;
        private Mock<ILogger<EmployeesController>> _loggerMock = null!;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _loggerMock = new Mock<ILogger<EmployeesController>>();
            _controller = new EmployeesController(_repoMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsOk_WithEmployees()
        {
            // Arrange
            var mockList = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Salary = 1000 },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Salary = 2000 }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(mockList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
        }

        [TestMethod]
        public async Task GetById_ReturnsNotFound_IfNotExists()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task Create_ReturnsCreatedAtAction_WhenValid()
        {
            // Arrange
            var dto = new EmployeeCreateDto
            {
                FirstName = "Ana",
                LastName = "Lopez",
                Email = "ana@correo.com",
                Phone = "123",
                HireDate = DateTime.Today,
                BirthDate = DateTime.Today.AddYears(-25),
                Salary = 2000,
                DepartmentId = 1,
                PositionId = 1
            };

            _repoMock.Setup(r => r.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
        }
    }
}
