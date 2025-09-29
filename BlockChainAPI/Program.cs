using BlockChain.Application.Implementation;
using BlockChain.Application.Interfaces;
using BlockChain.Infrastructure.Data;
using BlockChain.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DbContext - use SQLite file database
var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connString));

// DI - repositories & services
builder.Services.AddScoped<IBlockchainRepository, BlockchainRepository>();
// Register service with HttpClientFactory so HttpClient is available
builder.Services.AddHttpClient<IBlockchainService, BlockchainService>(client =>
{
    client.BaseAddress = new Uri("https://api.blockcypher.com/v1/");
    client.DefaultRequestHeaders.Add("User-Agent", "ICMarkets-Blockchain-API/1.0");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Health checks
builder.Services.AddHealthChecks();

// Adding CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ICMarkets Blockchain API",
        Version = "v1",
        Description = "API for storing and retrieving blockchain data from BlockCypher"
    });
});

// Logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

