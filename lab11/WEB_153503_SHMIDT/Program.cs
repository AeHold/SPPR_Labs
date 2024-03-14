using Microsoft.AspNetCore.Authentication;
using NuGet.Packaging;
using Serilog;
using WEB_153503_SHMIDT.Domain.Models;
using WEB_153503_SHMIDT.Middleware;
using WEB_153503_SHMIDT.Models;
using WEB_153503_SHMIDT.Services.CartService;
using WEB_153503_SHMIDT.Services.CocktailTypeService;
using WEB_153503_SHMIDT.Services.CocktailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//builder.Services.AddScoped<ICocktailTypeService, MemoryCocktailTypeService>();
//builder.Services.AddScoped<ICocktailService, MemoryCocktailService>();

UriData uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;

builder.Services.AddScoped<Cart, SessionCart>();

builder.Services.AddHttpClient<ICocktailService, ApiCocktailService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));
builder.Services.AddHttpClient<ICocktailTypeService, ApiCocktailTypeService>(opt => opt.BaseAddress = new Uri(uriData.ApiUri));

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "cookie";
    opt.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
        // Получить Claims пользователя
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.SaveTokens = true;
    });

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapRazorPages().RequireAuthorization();

app.UseMiddleware<LoggingMiddleware>(logger);

app.Run();
