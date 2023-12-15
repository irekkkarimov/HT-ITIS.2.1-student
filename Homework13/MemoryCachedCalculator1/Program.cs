using MemoryCachedCalculator.Configuration;
using MemoryCachedCalculator.Services;
using MemoryCachedCalculator.Services.CachedCalculator;
using MemoryCachedCalculator.Services.MathCalculator;
using MemoryCachedCalculator.Services.MyMemoryCache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMyMemoryCache, MyMemoryCache>();
builder.Services.AddMathCalculator();
builder.Services.AddCachedMathCalculator();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();