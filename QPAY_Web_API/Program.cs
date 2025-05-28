
using QPay.API;
using QPay.API.Extensions;
using QPay.DAL.Repository;

var builder = WebApplication.CreateBuilder(args);
// Access IConfiguration from the builder
IConfiguration configuration = builder.Configuration;
builder.Services.AddConfig(configuration);
builder.Services.AddControllers();
builder.Services.AddSingleton<DbRepository>();
//builder.Services.AddTransient<ILoginRepository, LoginRepository>();

builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();



//builder.Services.AddAuthorization();

//builder.Services.Configure<OAuthSettings>(builder.Configuration.GetSection("OAuthSettings"));
//builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

//builder.Services.AddHttpClient("ApiClient", client =>
//{
//    var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
//    client.BaseAddress = new Uri(apiSettings.BaseUrl);
//})
//.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
//{
//    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
//});

//builder.Services.AddScoped<ApiService>();

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

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => Results.Ok("Api is Working"));
app.UseMiddleware<WrapperResponse>();
app.UseCors("CorsPolicy");
app.UseStaticFiles();
app.UseRouting();
app.MapFallbackToFile("index.html");
app.UseHttpsRedirection();
app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller}/{action}/{id?}");
app.Run();


