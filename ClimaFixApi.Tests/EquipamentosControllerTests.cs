using ClimaFixApi.Controllers;
using ClimaFixApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MongoDB.Driver;
using Xunit;

namespace ClimaFixApi.Tests;

public class EquipamentosControllerTests
{
    [Fact]
    public async Task GetAllEquipamentos_DeveRetornarOk_ComListaDeEquipamentos()
    {
        // Arrange
        var mockDatabase = new Mock<IMongoDatabase>();
        var mockCollection = new Mock<IMongoCollection<EquipamentoClima>>();

        mockDatabase.Setup(db => db.GetCollection<EquipamentoClima>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                    .Returns(mockCollection.Object);

        var controller = new EquipamentosController(mockDatabase.Object);

        // Act
        var result = await controller.GetAllEquipamentos();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}