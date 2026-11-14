 using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClimaFixApi.Models;

public class EquipamentoClima
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; } = string.Empty;

    [BsonElement("tipo")]
    public string Tipo { get; set; } = string.Empty; // Ex: Ar-condicionado, Tubulação de Cobre, Elétrica

    [BsonElement("preco")]
    public decimal Preco { get; set; }

    [BsonElement("descricao")]
    public string Descricao { get; set; } = string.Empty;
}
