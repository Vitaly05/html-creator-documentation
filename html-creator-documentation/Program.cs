var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseRouting();

app.UseStaticFiles();

app.MapControllers();

app.MapRazorPages();


app.MapGet("/", async (context) => await context.Response.SendFileAsync("wwwroot/index.html"));
//app.MapGet("/docs", async (context) => await context.Response.SendFileAsync("wwwroot/docs.html"));

app.Run();