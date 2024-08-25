using AccommodationService.Application.Repositories;
using AccommodationService.Application.Services;
using AccommodationService.Authorization;
using AccommodationService.Configuration;
using AccommodationService.Infrastructure;
using AccommodationService.Infrastructure.Repositories;
using AccommodationService.Infrastructure.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text.Json.Serialization;
using Serilog;

var AllowAllOrigins = "_AllowAllOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        AllowAllOrigins,
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<AccommodationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

var firebaseProjectId = builder.Configuration["FirebaseAuthClientConfig:ProjectId"];

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = "https://securetoken.google.com/" + firebaseProjectId,
    ValidateAudience = true,
    ValidAudience = firebaseProjectId,
    ValidateLifetime = true,
};

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://securetoken.google.com/" + firebaseProjectId;
        options.TokenValidationParameters = tokenValidationParameters;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<FirebaseAuthClientConfig>(builder.Configuration.GetSection("FirebaseAuthClientConfig"));

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!string.IsNullOrWhiteSpace(builder.Configuration.GetSection("ElasticApm").GetValue<string>("ServerCert")))
{
    builder.Services.AddElasticApm();
}

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});


builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IAvailabilityPeriodService, AvailabilityPeriodService>();

builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IAvailabilityPeriodRepository, AvailabilityPeriodRepository>();

builder.Services.AddScoped<IAuthorizationHandler, AuthorizationLevelAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseSwagger((opt) =>
{
    opt.RouteTemplate = "swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI();
app.UseMiddleware<RequestContextLoggingMiddleware>();
app.UseSerilogRequestLogging();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(AllowAllOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.MapDefaultControllerRoute();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<AccommodationDbContext>();
context.Database.Migrate();


app.Run();