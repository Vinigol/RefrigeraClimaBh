using ClimaFixApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os controladores e serviços da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Leitura da string de conexão (compatível com appsettings.json ou variável de ambiente)
var mongoUri = builder.Configuration["ConnectionStrings:DefaultConnection"] 
               ?? builder.Configuration["MONGODB_URI"];

if (string.IsNullOrEmpty(mongoUri))
{
    throw new InvalidOperationException("A string de conexão do MongoDB não foi encontrada.");
}

// Registro do MongoClient e do banco de dados no container de injeção de dependência
var mongoClient = new MongoClient(mongoUri);
var mongoDatabase = mongoClient.GetDatabase("climafix");

builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton(mongoDatabase);

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();