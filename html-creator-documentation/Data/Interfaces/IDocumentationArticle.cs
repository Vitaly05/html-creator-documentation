using html_creator_documentation.Models;

namespace html_creator_documentation.Data.Interfaces
{
    public interface IDocumentationArticle
    {
        List<ArticleElement> GetArticleElementsFrom(string name);
        void UpdateArticle(string name, List<ArticleElement> articleElements, Action onSuccess, Action onError);
    }
}
