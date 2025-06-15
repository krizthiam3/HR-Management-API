using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using HRManagement.API.Controllers;
using HRManagement.API.Repositories.Interfaces;
using HRManagement.API.Models;
using HRManagement.API.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRManagement.Tests.Controllers;

[TestClass]
public class PositionsControllerTests
{
    private Mock<IPositionRepository> _repoMock = null!;
    private Mock<ILogger<PositionsController>> _loggerMock = null!;
    private PositionsController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _repoMock = new Mock<IPositionRepository>();
        _loggerMock = new Mock<ILogger<PositionsController>>();
        _controller = new PositionsController(_repoMock.Object, _loggerMock.Object);
    }

    [TestMethod]
    public async Task GetAll_ReturnsOk()
    {
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Position>());
        var result = await _controller.GetAll();
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task GetById_ReturnsNotFound_WhenMissing()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Position?)null);
        var result = await _controller.GetById(99);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task Create_ReturnsCreatedAtAction_WhenValid()
    {
        var dto = new PositionCreateDto { Title = "Developer", Description = "Writes code" };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Position>())).Returns(Task.CompletedTask);

        var result = await _controller.Create(dto);
        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
    }
}
