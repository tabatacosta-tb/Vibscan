
using VendasLux.API.Extensions;
using VendasLux.API.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o suporte Controllers
builder.Services.AddControllers();

// Configura o Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SchemaFilter<PedidoRequestSchemaFilter>();
});

// CHAMA DEPENDÊNCIA
builder.Services.ResolveDependencies();

var app = builder.Build();

// Habilita o Swagger apenas no ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); // Mapeia as rotas automaticamente

app.Run();