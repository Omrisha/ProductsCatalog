using ApplicationModel;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppExtensions(builder.Configuration);

WebApplication app = builder.Build();

app.ConfigureWebApplication();

app.Run();