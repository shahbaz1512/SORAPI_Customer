using DAL;
using Serilog;
using Serilog.Events;
using SORAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using SORAPI.Interface;
using SORAPI.DALC;
using SORAPI.DataValidator;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Serilog with separate asynchronous file sinks for each log level


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .WriteTo.Async(a => a.Logger(lc => lc
        .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Information)
        .WriteTo.File(configuration["Serilog:WriteTo:1:Args:path"]  , rollingInterval: RollingInterval.Hour, outputTemplate: "{Timestamp:yyyy-MM-dd HH} [{Level}] {Message}{NewLine}{Exception}")))
    .WriteTo.Async(a => a.Logger(lc => lc
        .Filter.ByIncludingOnly(le => le.Level == LogEventLevel.Error)
        .WriteTo.File(configuration["Serilog:WriteTo:1:Args:path"], rollingInterval: RollingInterval.Hour, outputTemplate: "{Timestamp:yyyy-MM-dd HH} [{Level}] {Message}{NewLine}{Exception}")))
    .CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ProcessAsyncTransactionInterface,DALC>();
builder.Services.AddTransient<Datavalidator, VaildateRequest>();

// for building windows servive
builder.Services.AddWindowsService();

// Add configuration files
builder.Configuration.AddJsonFile("appsettings.shared.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Load the connection string from the configuration
ConfigDb.Constring = builder.Configuration.GetConnectionString("Connection");
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
var appSettings = appSettingsSection.Get<AppSettings>();
ConfigReader.AppSettings = appSettings;

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var Port = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["Port"];

    serverOptions.ListenAnyIP(Convert.ToInt16(Port)); // Change the port here
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.MapGet("/", async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("wwwroot/index.html");
});
app.MapControllers();
app.Run();
