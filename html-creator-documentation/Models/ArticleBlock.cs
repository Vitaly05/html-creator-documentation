using html_creator_documentation.Data;

namespace html_creator_documentation.Models
{
	public class ArticleBlock
	{
		public string Type { get; set; } = "";
		public List<ArticleElement> Elements { get; set; } = new List<ArticleElement>();

		public bool IsMain
		{
			get
			{
				return Type == ArticleBlocksTypes.Main;
			}
		}
	}
}
