using html_creator_documentation.Data;
using html_creator_documentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Xml.Linq;

namespace html_creator_documentation.Pages
{
    public class DocsModel : PageModel
    {
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


        public void OnGet(string topic)
        {
            try
            {
                //HttpContext.Session.SetString("access", "admin");
                if (HttpContext.Session.Keys.Contains("access"))
                    if (HttpContext.Session.GetString("access").Equals("admin"))
                        CanEdit = true;
            }
            catch { }

            CanEdit = true;
            GetArticle(topic);
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
            try
            {
                using (StreamReader sr = new($"Data/Articles/{topic}.json"))
                {
                    ArticleElements = JsonSerializer.Deserialize<List<ArticleElement>>(sr.ReadToEnd()) ?? new();
                }
            }
            catch { }
        }
    }
}