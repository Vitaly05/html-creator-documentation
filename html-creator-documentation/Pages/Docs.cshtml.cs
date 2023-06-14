using html_creator_documentation.Data;
using html_creator_documentation.Data.Interfaces;
using html_creator_documentation.Models;
using html_creator_documentation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using System.Xml.Linq;

namespace html_creator_documentation.Pages
{
    public class DocsModel : PageModel
    {
        public DocsModel(IDocumentationArticle documentationContext,
            JwtService jwtService)
        {
            _documentationContext = documentationContext;
            _jwtService = jwtService;
        }

        private IDocumentationArticle _documentationContext;
        private JwtService _jwtService;


        public string Title { get; set; } = "";
        public List<ArticleElement> ArticleElements { get; set; } = new();

        public bool HasTitle
        {
            get
            {
                foreach (var element in ArticleElements)
                    if (element.Type == ArticleElementsTypes.LargeBlock)
                        return true;
                return false;
            }
        }

        public bool CanEdit { get; set; } = false;


        public List<ArticleElement> GetTitles()
        {
            return (from e in ArticleElements
                    where e.Type == ArticleElementsTypes.LargeBlock
                    select e).ToList();
        }


        public void OnGet(string topic, string accessToken)
        {
            if (accessToken is not null)
            {
                var decodedToken = _jwtService.DecodeJwtToken(accessToken);
                if (decodedToken is not null)
                {
                    if (decodedToken.FindFirstValue("access") == AuthOptions.ACCESS_KEY)
                    {
                        CanEdit = true;
                    }
                }
            }

            GetArticle(topic);
        }

        public IActionResult OnPost([FromBody] ArticleElement articleElement)
        {
            var htmlArticleElement = Partial("_ArticleElement", new ArticleElementModel
            {
                CanEdit = true,
                ArticleElement = articleElement
            });
            return htmlArticleElement;
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
            Title = topic;
            ArticleElements = _documentationContext.GetArticleElementsFrom(topic);
        }
    }
}