// dotcover disable
using System.Diagnostics.CodeAnalysis;
using MemoryCachedCalculator.Configuration;
using MemoryCachedCalculator.Services.MyMemoryCache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddSingleton<IMyMemoryCache, MyMemoryCache>()
    .AddMathCalculator()
    .AddCachedMathCalculator();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Calculator}/{action=Index}/{id?}");
app.Run();

namespace MemoryCachedCalculator
{
    [ExcludeFromCodeCoverage]
    public partial class Program { }
}