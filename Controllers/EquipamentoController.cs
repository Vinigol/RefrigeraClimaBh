using ClimaFixApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ClimaFixApi.Controllers;

[ApiController]
[Route("api/v1/equipamentos")]
public class EquipamentosController : ControllerBase
{
    private readonly IMongoCollection<EquipamentoClima> _equipamentosCollection;

    public EquipamentosController(IMongoDatabase database)
    {
        _equipamentosCollection = database.GetCollection<EquipamentoClima>("equipamentos");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEquipamentos()
    {
        var equipamentos = await _equipamentosCollection.Find(new BsonDocument()).ToListAsync();
        return Ok(equipamentos);
    }

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetEquipamentoById(string id)
    {
        var equipamento = await _equipamentosCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
        if (equipamento == null)
        {
            return NotFound();
        }
        return Ok(equipamento);
    }

    [HttpPost]
    [Authorize] // Exige login (Admin ou Tecnico) para criar
    public async Task<IActionResult> CreateEquipamento([FromBody] EquipamentoClima equipamento)
    {
        await _equipamentosCollection.InsertOneAsync(equipamento);
        return CreatedAtAction(nameof(GetEquipamentoById), new { id = equipamento.Id }, equipamento);
    }

    [HttpPut("{id:length(24)}")]
    [Authorize] // Exige login para atualizar
    public async Task<IActionResult> UpdateEquipamento(string id, [FromBody] EquipamentoClima updatedEquipamento)
    {
        updatedEquipamento.Id = id;
        var result = await _equipamentosCollection.ReplaceOneAsync(e => e.Id == id, updatedEquipamento);
        if (result.MatchedCount == 0)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    [Authorize(Roles = "Admin")] // RECURSO RESTRITO: Apenas perfil Admin pode apagar!
    public async Task<IActionResult> DeleteEquipamento(string id)
    {
        var result = await _equipamentosCollection.DeleteOneAsync(e => e.Id == id);
        if (result.DeletedCount == 0)
        {
            return NotFound();
        }
        return NoContent();
    }
}