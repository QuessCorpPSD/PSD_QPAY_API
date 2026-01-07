
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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
//LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config") // Loads from project root
    .GetCurrentClassLogger();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter("dd-MM-yyyy hh:mm:ss tt"));
    });
//builder.Logging.ClearProviders();
//builder.Logging.AddConsole();            // Logs to console
//builder.Logging.AddDebug();              // Logs to Visual Studio Output window
//builder.Logging.AddEventLog();

// Optional: set minimum log level
//builder.Logging.SetMinimumLevel(LogLevel.Information);
// Access IConfiguration from the builder
IConfiguration configuration = builder.Configuration;
builder.Services.AddConfig(configuration);
builder.Services.AddControllers();
builder.Services.AddSingleton<DbRepository>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
//builder.Services.AddTransient<ILoginRepository, LoginRepository>();

builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();



builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
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

        // Optional: log reason for failure
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Token failed: " + context.Exception.Message);
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        );
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1073741824; // 1 GB = 1024 * 1024 * 1024 bytes
});
// Add services to the container.
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);
var app = builder.Build();
//app.UseMiddleware<ValidationJwtMiddleware>();
app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFi`leProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "ReimbursementPolicyUploads")),
//    RequestPath = "/ReimbursementPolicyUploads",
//    ServeUnknownFileTypes = true,
//    DefaultContentType = "application/octet-stream"
//});
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "GST_Certificate")),
//    RequestPath = "/GST_Certificate",
//    ServeUnknownFileTypes = true,
//    DefaultContentType = "application/octet-stream"
//});
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "SEZ_Certificate")),
//    RequestPath = "/SEZ_Certificate",
//    ServeUnknownFileTypes = true,
//    DefaultContentType = "application/octet-stream"
//});
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "LUT_Certificate")),
//    RequestPath = "/LUT_Certificate",
//    ServeUnknownFileTypes = true,
//    DefaultContentType = "application/octet-stream"
//});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<WrapperResponse>();
app.UseCors("CorsPolicy");
app.MapFallbackToFile("index.html");
app.UseHttpsRedirection();
//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async context =>
//    {
//        context.Response.StatusCode = 500;
//        context.Response.ContentType = "application/json";

//        var error = context.Features.Get<IExceptionHandlerPathFeature>();
//        if (error != null)
//        {
//            await context.Response.WriteAsync(JsonSerializer.Serialize(new
//            {
//                error = error.Error.Message
//            }));
//        }
//    });
//});
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

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action}/{id?}");
app.Run();


