namespace html_creator_documentation.Models
{
    public class ArticleElement
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public List<string> Elements { get; set; } = new List<string>();
        public List<string> CodeLines { get; set; } = new List<string>();

        public ArticleElement(string type, string text)
        {
            Type = type;
            Text = text;
        }
    }
}
