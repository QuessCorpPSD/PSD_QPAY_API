using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using NLog;
using QPay.API;
using QPay.API.Extensions;
using QPay.API.LoggerService;
using QPay.API.Models;
using QPay.DAL.Repository;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

/* ============================
   LOGGING (NLog)
============================ */
var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

/* ============================
   CONTROLLERS & JSON
============================ */
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new DateTimeConverter("dd-MM-yyyy hh:mm:ss tt"));
    });

/* ============================
   CONFIG & SERVICES
============================ */
IConfiguration configuration = builder.Configuration;

builder.Services.AddConfig(configuration);
builder.Services.AddSingleton<DbRepository>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
SelectPdf.GlobalProperties.LicenseKey = builder.Configuration["SelectPdfLicenseKey"];
/* ============================
   JWT AUTHENTICATION
============================ */
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

builder.Services.AddSingleton(jwtSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("❌ Token failed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("✅ Token validated successfully.");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

/* ============================
   CORS (ANGULAR)
============================ */
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

/* ============================
   FILE UPLOAD LIMIT
============================ */
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1073741824; // 1 GB
});

/* ============================
   BUILD APP
============================ */
var app = builder.Build();

/* ============================
   MIDDLEWARE PIPELINE
============================ */

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

/* ✅ CORS MUST BE HERE */
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

/* Optional Response Wrapper */
app.UseMiddleware<WrapperResponse>();

/* ============================
   CONTROLLERS
============================ */
app.MapControllers();

/* ============================
   ROUTING
============================ */
app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action}/{id?}");

app.MapFallbackToFile("index.html");

/* ============================
   NO-CACHE HEADERS
============================ */
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
        ctx.Context.Response.Headers.Append("Pragma", "no-cache");
        ctx.Context.Response.Headers.Append("Expires", "0");
    }
});

app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";

    await next();
});

app.Run();
