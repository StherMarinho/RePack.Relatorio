using ApiRelatorio.Repositories;
using ApiRelatorio.Repositories.Interfaces;
using ApiRelatorio.Services;
using ApiRelatorio.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de dependência
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();

// CORS — permite requisições do frontend React
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PermitirFrontend"); // deve vir antes de UseAuthorization

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();