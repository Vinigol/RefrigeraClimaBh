using ClimaFixApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ClimaFixApi.Controllers;

[ApiController]
[Route("api/v1/ordens-manutencao")]
public class OrdensManutencaoController : ControllerBase
{
    private readonly IMongoCollection<OrdemManutencao> _ordensCollection;
    private readonly IMongoCollection<EquipamentoClima> _equipamentosCollection;

    public OrdensManutencaoController(IMongoDatabase database)
    {
        _ordensCollection = database.GetCollection<OrdemManutencao>("ordens_manutencao");
        _equipamentosCollection = database.GetCollection<EquipamentoClima>("equipamentos");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrdens()
    {
        var ordens = await _ordensCollection.Find(new BsonDocument()).ToListAsync();
        return Ok(ordens);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrdem([FromBody] OrdemManutencao ordem)
    {
        // 1. Validação de Relacionamento (1 para N): O Equipamento existe?
        if (!ObjectId.TryParse(ordem.EquipamentoId, out var equipamentoIdObj))
        {
            return BadRequest(new { mensagem = "ID de equipamento inválido." });
        }

        var equipamentoExiste = await _equipamentosCollection
            .Find(e => e.Id == ordem.EquipamentoId)
            .FirstOrDefaultAsync();

        if (equipamentoExiste == null)
        {
            return NotFound(new { mensagem = "Equipamento não encontrado. Não é possível vincular a ordem de manutenção." });
        }

        // 2. Regra de Negócio: Conflito de Agenda do Técnico
        // Verifica se já existe ordem para o mesmo técnico na mesma data/hora exata
        var conflitoAgenda = await _ordensCollection
            .Find(o => o.TecnicoId == ordem.TecnicoId && o.DataServico == ordem.DataServico)
            .FirstOrDefaultAsync();

        if (conflitoAgenda != null)
        {
            return BadRequest(new { mensagem = "Conflito de agenda: O técnico já possui uma ordem de serviço agendada para este horário." });
        }

        // 3. Regra de Negócio: Desconto na Manutenção Preventiva
        if (ordem.Tipo.Equals("Preventiva", StringComparison.OrdinalIgnoreCase))
        {
            // Aplica 10% de desconto para manutenção preventiva
            ordem.ValorFinal = ordem.ValorBase * 0.90m;
        }
        else
        {
            // Corretiva ou outros tipos mantêm o valor base integral
            ordem.ValorFinal = ordem.ValorBase;
        }

        await _ordensCollection.InsertOneAsync(ordem);
        return CreatedAtAction(nameof(GetAllOrdens), new { id = ordem.Id }, new { mensagem = "Ordem de manutenção criada com sucesso!", ordem });
    }
}