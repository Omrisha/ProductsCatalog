using ApplicationModel;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppExtensions(builder.Configuration);

WebApplication app = builder.Build();

app.ConfigureWebApplication();

app.Run();