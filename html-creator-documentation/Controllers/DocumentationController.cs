using html_creator_documentation.Data;
using html_creator_documentation.HtmlElements;
using html_creator_documentation.Models;
using html_creator_documentation.Repositories.Interfaces;
using html_creator_documentation.Services;
using html_creator_library.HeadComponents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace html_creator_documentation.Controllers
{
    [ApiController]
    [Route("documentation")]
    public class DocumentationController : Controller
    {
        private IDocumentationArticleRepository _documentationContext;
        private JwtService _jwtService;
        private ArticleElementsCreator _articleElementsCreator;

        private DocumentationModel model = new();

        public DocumentationController(IDocumentationArticleRepository documentationContext,
            JwtService jwtService,
            ArticleElementsCreator articleElementsCreator)
        {
            _documentationContext = documentationContext;
            _jwtService = jwtService;
            _articleElementsCreator = articleElementsCreator;
        }


        [HttpGet]
        public async Task<IActionResult> GetDocumentation(string? topic, string? accessToken)
        {
            if (accessToken is not null)
            {
                var decodedToken = _jwtService.DecodeJwtToken(accessToken);
                if (decodedToken is not null)
                {
                    if (decodedToken.FindFirstValue("access") == AuthOptions.ACCESS_KEY)
                    {
                        model.CanEdit = true;
                    }
                }
            }

            GetArticle(topic);
            await Response.WriteAsync(_articleElementsCreator.CreateDocumentationView(model));
            return Ok();
        }

        [HttpGet("getToken")]
        public IActionResult GetToken(string login, string password)
        {
            var token = _jwtService.EncodeJwtToken(login, password);
            if (token is null) return BadRequest("Неверный логин или пароль");
            return Json(token);
        }

        [Authorize]
        [HttpPost("createElement")]
        public IActionResult CreateElement([FromBody] ArticleElement articleElement)
        {
            var html = _articleElementsCreator.GetArticleElementHtml(new ArticleElementModel
            {
                CanEdit = true,
                ArticleElement = articleElement
            });
            return Ok(html);
        }

        [Authorize]
        [HttpPost("update/{article}")]
        public async Task<IActionResult> UpdateArticle(string article, [FromBody] List<ArticleElement> articleElements)
        {
            TaskCompletionSource<IActionResult> taskCompletionSource = new();

            _documentationContext.UpdateArticle(article, articleElements,
                onSuccess: () => taskCompletionSource.TrySetResult(Ok()),
                onError: () => taskCompletionSource.TrySetResult(StatusCode(500)));

            IActionResult status = await taskCompletionSource.Task;
            return status;
        }



        private void GetArticle(string topic)
        {
            switch (topic)
            {
                default:
                case Topics.Start:
                    LoadArticle(Topics.Start);
                    break;
                case Topics.Namespaces:
                    LoadArticle(Topics.Namespaces);
                    break;
                case Topics.StaticClasses:
                    LoadArticle(Topics.StaticClasses);
                    break;
                case Topics.Examples:
                    LoadArticle(Topics.Examples);
                    break;
            }
        }

        private void LoadArticle(string topic)
        {
            model.Title = topic;
            model.ArticleElements = _documentationContext.GetArticleElementsFrom(topic);
        }
    }
}
