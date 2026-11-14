using ClimaFixApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ClimaFixApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipamentoController : ControllerBase
{
    private readonly IMongoCollection<EquipamentoClima> _equipamentosCollection;

    public EquipamentoController(IMongoDatabase database)
    {
        _equipamentosCollection = database.GetCollection<EquipamentoClima>("equipamentos");
    }

    [HttpGet]
    public async Task<ActionResult<List<EquipamentoClima>>> Get()
    {
        var equipamentos = await _equipamentosCollection.Find(_ => true).ToListAsync();
        return Ok(equipamentos);
    }

    [HttpPost]
    public async Task<ActionResult> Post(EquipamentoClima novoEquipamento)
    {
        await _equipamentosCollection.InsertOneAsync(novoEquipamento);
        return Ok(new { mensagem = "Equipamento cadastrado com sucesso!", novoEquipamento });
    }
}