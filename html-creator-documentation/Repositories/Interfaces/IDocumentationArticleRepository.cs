using html_creator_documentation.Models;

namespace html_creator_documentation.Repositories.Interfaces
{
    public interface IDocumentationArticleRepository
    {
        List<ArticleElement> GetArticleElementsFrom(string name);
        void UpdateArticle(string name, List<ArticleElement> articleElements, Action onSuccess, Action onError);
    }
}
