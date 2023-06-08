using html_creator_documentation.Data;
using System.Text.Json.Serialization;

namespace html_creator_documentation.Models
{
    public class ArticleElement
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Value { get; set; } = "";
        public List<string> ListValues { get; set; } = new();
        [JsonPropertyName("Elements")]
        public List<ArticleElement> InnerElements { get; set; } = new();
    }
}
