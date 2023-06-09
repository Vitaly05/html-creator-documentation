using html_creator_documentation.Data.Interfaces;
using html_creator_documentation.Models;
using System.Text;
using System.Text.Json;

namespace html_creator_documentation.Services
{
    public class FileDocumentationService : IDocumentationArticle
    {
        public List<ArticleElement> GetArticleElementsFrom(string name)
        {
            try
            {
                using (StreamReader sr = new($"Data/Articles/{name}.json"))
                {
                    return JsonSerializer.Deserialize<List<ArticleElement>>(sr.ReadToEnd()) ?? new();
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Не удалось открыть файл: {ex.FileName}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Не удалось десериализовать список элементов из \"{name}\": {ex.Message}");
            }

            return new List<ArticleElement>();
        }

        public async void UpdateArticle(string name, List<ArticleElement> articleElements, Action onSuccess, Action onError)
        {
            try
            {
                using (FileStream fs = new FileStream($"Data/Articles/{name}.json", FileMode.Create, FileAccess.Write))
                {
                    byte[] data = Encoding.Default.GetBytes(JsonSerializer.Serialize(articleElements));
                    await fs.WriteAsync(data, 0, data.Length);
                    onSuccess.Invoke();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                onError.Invoke();
            }
        }
    }
}
