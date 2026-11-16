using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClimaFixApi.Models;

public class OrdemManutencao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("equipamentoId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string EquipamentoId { get; set; } = string.Empty;

    [BsonElement("tecnicoId")]
    public string TecnicoId { get; set; } = string.Empty; // Usado para validar conflito de agenda

    [BsonElement("dataServico")]
    public DateTime DataServico { get; set; }

    [BsonElement("tipo")] // Ex: "Preventiva" ou "Corretiva"
    public string Tipo { get; set; } = string.Empty;

    [BsonElement("valorBase")]
    public decimal ValorBase { get; set; }

    [BsonElement("valorFinal")]
    public decimal ValorFinal { get; set; } // Onde aplicaremos a regra de desconto

    [BsonElement("descricao")]
    public string Descricao { get; set; } = string.Empty;
}