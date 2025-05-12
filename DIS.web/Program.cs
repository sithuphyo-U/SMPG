using DIS.DataAccess;
using DIS.Infrastructure.Utilities;
using DIS.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Reflection;
using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseKestrel(opt =>
//{
//    opt.ListenAnyIP(8080);
//});


//// database
//builder.Services.AddDbContext<DatabaseContext>(options =>
//    options.UseLazyLoadingProxies().UseSqlServer(ConfigManager.GetConnectionString(), sqlServerOptionsAction: sqlOptions =>
//    {
//        sqlOptions.EnableRetryOnFailure(
//            maxRetryCount: 3,
//            maxRetryDelay: TimeSpan.FromSeconds(3),
//            errorNumbersToAdd: null);
//    }).EnableDetailedErrors()
//);

//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(opt => { });
//builder.Services.AddControllers();

////register add scope interface and classes
//builder.Services.(Assembly.GetExecutingAssembly());
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddHttpClient();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
//    {
//        Title = "Document Management System",
//        Version = "v1",
//    });
//    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
//    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
//    {
//        Name = "Authorization",
//        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
//        Scheme = "Bearer",
//        BearerFormat = "JWT",
//        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
//        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
//    });
//});
//builder.Services.AddSignalR();
////to get HttpContext.Request values 
////builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
////Get Sercret Key
//var appSettingSection = builder.Configuration.GetSection("AppSettings");

//var key = Encoding.ASCII.GetBytes(ConfigManager.GetSecretKey());
//// configure jwt authentication
//// Register Authentication with JWT 
//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{
//    x.RequireHttpsMetadata = false;
//    x.SaveToken = true;
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});
//// Add CORS with allow specific origin
////change if needed
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//    {
//        builder.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:8082", "http://192.168.40.2:8082")
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials(); // Allow credentials for SignalR
//    });
//});
//builder.Logging.AddLog4Net();
//builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\Keys"))
//    .SetApplicationName("DMS.Web");
//var app = builder.Build();

//// Configure the swagger page 
////this is for development and production (change if needed)
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger().UseAuthentication();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
//    });

//}
//// Middleware to measure request processing time
//app.Use(async (context, next) =>
//{
//    var stopwatch = Stopwatch.StartNew();
//    await next.Invoke();
//    stopwatch.Stop();

//    var logger = app.Services.GetRequiredService<ILogger<Program>>();
//    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} - {stopwatch.ElapsedMilliseconds}ms");
//});

//// Middleware to monitor memory usage
//app.Use(async (context, next) =>
//{
//    long memoryBefore = Process.GetCurrentProcess().PrivateMemorySize64;
//    await next.Invoke();
//    long memoryAfter = Process.GetCurrentProcess().PrivateMemorySize64;

//    var logger = app.Services.GetRequiredService<ILogger<Program>>();
//    logger.LogInformation($"Memory Usage Before: {memoryBefore / 1024 / 1024} MB, After: {memoryAfter / 1024 / 1024} MB");
//});
//////to create model to database table
////using (var serviceScope = app.Services.CreateScope())
////{
////    var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
////    //context.Database.Migrate();
////    context.Database.EnsureCreated();
////    RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
////    context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
////    databaseCreator.CreateTables();
////    context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
////}
//// Use CORS
//app.UseCors("AllowAll");
//app.UseSession();
//app.UseRouting();
//// Enable authentication middleware
//app.UseAuthentication();
////enable authorization middleware
//app.UseAuthorization();

//app.MapControllers();
//app.MapHub<EventHub>("/hub");
////app.UseEndpoints(
////    endpoints => endpoints.MapHub<EventHub>("/hub"));
//app.Run();

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(opt =>
{
    opt.ListenAnyIP(5084);
});

////sql server
// DIS Database
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(ConfigManager.GetConnectionString(), sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(3),
            errorNumbersToAdd: null);
    }).EnableDetailedErrors() //console will print details error ( useful in development time )
);


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opt => { });
builder.Services.AddControllers();
//register add scope interface and classes
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Disaster Information System",
        
    });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...\""
    });
});
builder.Services.AddSignalR();
//to get HttpContext.Request values 
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//Get Sercret Key
var appSettingSection = builder.Configuration.GetSection("AppSettings");

var key = Encoding.ASCII.GetBytes(ConfigManager.GetSecretKey());
// configure jwt authentication
// Register Authentication with JWT 
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
// Add CORS with allow specific origin
//change if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:8082", "http://192.168.40.2:8082")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Allow credentials for SignalR
    });
});
builder.Logging.AddLog4Net();
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(@"C:\Keys"))
    .SetApplicationName("DIS.Web");
var app = builder.Build();

// Configure the swagger page 
//this is for development and production (change if needed)
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger().UseAuthentication();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });

}
// Middleware to measure request processing time
app.Use(async (context, next) =>
{
    var stopwatch = Stopwatch.StartNew();
    await next.Invoke();
    stopwatch.Stop();

    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} - {stopwatch.ElapsedMilliseconds}ms");
});

// Middleware to monitor memory usage
app.Use(async (context, next) =>
{
    long memoryBefore = Process.GetCurrentProcess().PrivateMemorySize64;
    await next.Invoke();
    long memoryAfter = Process.GetCurrentProcess().PrivateMemorySize64;

    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Memory Usage Before: {memoryBefore / 1024 / 1024} MB, After: {memoryAfter / 1024 / 1024} MB");
});
////to create model to database table
//using (var serviceScope = app.Services.CreateScope())
//{
//    var context = serviceScope.ServiceProvider.GetRequiredService<DatabaseContext>();
//    //context.Database.Migrate();
//    context.Database.EnsureCreated();
//    RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
//    context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
//    databaseCreator.CreateTables();
//    context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
//}
// Use CORS
app.UseCors("AllowAll");
app.UseSession();
app.UseRouting();
// Enable authentication middleware
app.UseAuthentication();
//enable authorization middleware
app.UseAuthorization();

app.MapControllers();
app.MapHub<EventHub>("/hub");
//app.UseEndpoints(
//    endpoints => endpoints.MapHub<EventHub>("/hub"));
app.Run();
