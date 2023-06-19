using html_creator_documentation.Data;

namespace html_creator_documentation.Models
{
    public class DocumentationModel
    {
        public string Title { get; set; } = "";
        public List<ArticleElement> ArticleElements { get; set; } = new();

        public bool HasTitle
        {
            get
            {
                foreach (var element in ArticleElements)
                    if (element.Type == ArticleElementsTypes.LargeBlock &&
                        !String.IsNullOrWhiteSpace(element.Title))
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
    }
}
