var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", async (context) => await context.Response.SendFileAsync("Root/index.html"));

app.Run();
