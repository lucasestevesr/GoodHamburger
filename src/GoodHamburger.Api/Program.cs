using System.Globalization;
using System.Text.Json.Serialization;
using GoodHamburger.Api.Extensions;
using GoodHamburger.Api.Middlewares;
using GoodHamburger.CrossCutting.IoC;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddApiValidation();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddApiAuthentication(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

var apiCulture = CultureInfo.InvariantCulture;
var apiMessageCulture = CultureInfo.GetCultureInfo("pt-BR");

CultureInfo.DefaultThreadCurrentCulture = apiCulture;
CultureInfo.DefaultThreadCurrentUICulture = apiMessageCulture;

if (app.Environment.IsDevelopment())
{
    await app.Services.ApplyDevelopmentMigrationsAndSeedAsync();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(apiCulture, apiMessageCulture),
    SupportedCultures = [apiCulture],
    SupportedUICultures = [apiMessageCulture],
    FallBackToParentCultures = false,
    FallBackToParentUICultures = true
});

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

if (builder.Configuration.GetValue("App:UseHttpsRedirection", true))
    app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
