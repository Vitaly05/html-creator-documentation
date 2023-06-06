namespace html_creator_documentation.Models
{
    public class ArticleElementModel
    {
        public bool CanEdit { get; set; } = false;
        public ArticleElement ArticleElement { get; set; } = new();
    }
}
