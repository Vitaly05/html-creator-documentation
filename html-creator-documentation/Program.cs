var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", async (context) => await context.Response.SendFileAsync("wwwroot/index.html"));
app.MapGet("/docs", async (context) => await context.Response.SendFileAsync("wwwroot/docs.html"));

app.Run();