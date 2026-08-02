using VibeScan.API.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ── Serviços ─────────────────────────────────────────────────────────────────
builder.Services
    .AddVibeScanDomain()
    .AddVibeScanAdapters(builder.Configuration)
    .AddVibeScanControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title       = "VibeScan API",
        Version     = "v1",
        Description = "Avaliador de código Vibe Coding com diagnóstico de arquitetura via IA"
    });
});

// CORS para o Blazor consumir a API localmente
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
        policy.WithOrigins("https://localhost:7200", "http://localhost:5200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// ── Pipeline ──────────────────────────────────────────────────────────────────
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VibeScan API v1"));
}

app.UseCors("BlazorPolicy");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
