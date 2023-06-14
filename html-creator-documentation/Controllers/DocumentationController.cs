using html_creator_documentation.Data;
using html_creator_documentation.Data.Interfaces;
using html_creator_documentation.Models;
using html_creator_documentation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace html_creator_documentation.Controllers
{
    [ApiController]
    [Route("documentation")]
    public class DocumentationController : Controller
    {
        private IDocumentationArticle _documentationContext;
        private JwtService _jwtService;

        public DocumentationController(IDocumentationArticle documentationContext,
            JwtService jwtService)
        {
            _documentationContext = documentationContext;
            _jwtService = jwtService;
        }


        [HttpGet("getToken")]
        public IActionResult GetToken(string login, string password)
        {
            var token = _jwtService.EncodeJwtToken(login, password);
            if (token is null) return BadRequest("Неверный логин или пароль");
            return Json(token);
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
    }
}
