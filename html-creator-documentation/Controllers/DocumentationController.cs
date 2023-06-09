using html_creator_documentation.Data.Interfaces;
using html_creator_documentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace html_creator_documentation.Controllers
{
    [ApiController]
    [Route("documentation")]
    public class DocumentationController : Controller
    {
        private IDocumentationArticle _documentationContext;

        public DocumentationController(IDocumentationArticle documentationContext)
        {
            _documentationContext = documentationContext;
        }


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
