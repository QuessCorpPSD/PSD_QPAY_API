
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using QPay.API;
using QPay.API.Extensions;
using QPay.API.Models;
using QPay.DAL.Repository;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
// Access IConfiguration from the builder
IConfiguration configuration = builder.Configuration;
builder.Services.AddConfig(configuration);
builder.Services.AddControllers();
builder.Services.AddSingleton<DbRepository>();
//builder.Services.AddTransient<ILoginRepository, LoginRepository>();

builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();



builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidIssuer = jwtSettings.Issuer,

//            ValidateAudience = true,
//            ValidAudience = jwtSettings.Audience,

//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

//            ValidateLifetime = true,
//            ClockSkew = TimeSpan.Zero
//        };

//        // Optional: log reason for failure
//        options.Events = new JwtBearerEvents
//        {
//            OnAuthenticationFailed = context =>
//            {
//                Console.WriteLine("Token failed: " + context.Exception.Message);
//                return Task.CompletedTask;
//            },
//            OnTokenValidated = context =>
//            {
//                Console.WriteLine("✅ Token validated successfully.");
//                return Task.CompletedTask;
//            }
//        };
//    });
//builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        );
});
// Add services to the container.
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddSingleton(jwtSettings);
var app = builder.Build();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "ReimbursementPolicyUploads")),
    RequestPath = "/ReimbursementPolicyUploads",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "GST_Certificate")),
    RequestPath = "/GST_Certificate",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "SEZ_Certificate")),
    RequestPath = "/SEZ_Certificate",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "LUT_Certificate")),
    RequestPath = "/LUT_Certificate",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});
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

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action}/{id?}");
app.Run();


