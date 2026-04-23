using GoodHamburger.Web.Auth;
using GoodHamburger.Web.Components;
using GoodHamburger.Web.Infrastructure.Http;
using GoodHamburger.Web.Orders;
using GoodHamburger.Web.Products;
using GoodHamburger.Web.Security;
using GoodHamburger.Web.Users;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped<AuthSession>();
builder.Services.AddScoped<AuthSessionStorage>();
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<OrdersApiClient>();
builder.Services.AddScoped<ProductsApiClient>();
builder.Services.AddScoped<UsersApiClient>();
builder.Services.AddHttpClient<ApiHttpClient>(client =>
{
    var baseUrl = builder.Configuration["Api:BaseUrl"]
        ?? throw new InvalidOperationException("Configuração 'Api:BaseUrl' não encontrada.");

    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

app.Logger.LogInformation("Configured Api:BaseUrl = {BaseUrl}", builder.Configuration["Api:BaseUrl"]);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
