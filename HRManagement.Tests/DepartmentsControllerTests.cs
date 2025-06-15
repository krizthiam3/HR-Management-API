using Moq;
using HRManagement.API.Controllers;
using HRManagement.API.Repositories.Interfaces;
using HRManagement.API.Models;
using HRManagement.API.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.Tests.Controllers;

[TestClass]
public class DepartmentsControllerTests
{
    private Mock<IDepartmentRepository> _repoMock = null!;
    private Mock<ILogger<DepartmentsController>> _loggerMock = null!;
    private DepartmentsController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _repoMock = new Mock<IDepartmentRepository>();
        _loggerMock = new Mock<ILogger<DepartmentsController>>();
        _controller = new DepartmentsController(_repoMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetAll_ReturnsOk()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Department>());
        var result = await _controller.GetAll();
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        _repoMock.Setup(r => r.GetByIdAsync(123)).ReturnsAsync((Department?)null);
        var result = await _controller.GetById(123);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task Create_ReturnsCreatedAtAction_WhenValid()
    {
        var dto = new DepartmentCreateDto { Name = "IT", Location = "HQ" };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Department>())).Returns(Task.CompletedTask);

        var result = await _controller.Create(dto);
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
    }
}
