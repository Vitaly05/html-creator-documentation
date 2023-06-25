using html_creator_documentation.Data;
using html_creator_documentation.HtmlElements;
using html_creator_documentation.Repositories;
using html_creator_documentation.Repositories.Interfaces;
using html_creator_documentation.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddMvc().AddRazorPagesOptions(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = AuthOptions.GetTokenValidationParameters();
    });

builder.Services.AddSingleton<IDocumentationArticleRepository, FileRepository>();
builder.Services.AddTransient<JwtService>();
builder.Services.AddTransient<ArticleElementsCreator>();

var app = builder.Build();

app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.UseSession();

app.MapGet("/", async (context) => await context.Response.SendFileAsync("wwwroot/index.html"));
app.MapGet("/auth", async (context) => await context.Response.SendFileAsync("wwwroot/auth.html"));

app.Run();