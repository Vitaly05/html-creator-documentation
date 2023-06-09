using html_creator_documentation.Data.Interfaces;
using html_creator_documentation.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSession();

builder.Services.AddSingleton<IDocumentationArticle, FileDocumentationService>();

var app = builder.Build();

app.UseRouting();

app.UseStaticFiles();

app.MapControllers();

app.MapRazorPages();

app.UseSession();

app.MapGet("/", async (context) => await context.Response.SendFileAsync("wwwroot/index.html"));

app.Run();