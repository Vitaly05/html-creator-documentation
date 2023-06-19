using html_creator_library;
using html_creator_library.BodyComponents;
using html_creator_library.BodyComponents.Containers;
using html_creator_documentation.Models;
using html_creator_documentation.Data;
using html_creator_library.HeadComponents;
using System.Reflection;
using System.Diagnostics;

namespace html_creator_documentation.HtmlElements
{
    public class ArticleElementsCreator
    {
        public string CreateDocumentationView(DocumentationModel model)
        {
            Head head = new Head();
            head.SetContext(
                new Meta("UTF-8"),
                new Meta().HttpEquiv("X-UA-Compatible", "IE=edge"),
                new Meta().Name("viewport", "width=device-width, initial-scale=1.0"),
                new Link("jQueryUI/jquery-ui.min.css", Relation.Stylesheet),
                new Script("jQuery/jquery.js"),
                new Script("jQuery/jquery.validate.min.js"),
                new Script("jQueryUI/jquery-ui.min.js"),
                new Link("style.css", Relation.Stylesheet),
                new Title($"HTML Creator Library - {model.Title}")
            );


            Header header = new Header(
                new HtmlAttribute().Class("default-padding flex jc-space-between ai-center"),
                new Div(
                    new HtmlAttribute().Class("flex"),
                    new ALink("/", new Div(
                        new HtmlAttribute().Class("logo flex ai-center clickable-element"),
                        new Image("Res/Logo.svg", "Logo"),
                        new Div(
                            new HtmlAttribute().Class("text"),
                            new Text("HTML Creator")
                        )
                    )),
                    new Div(
                        new HtmlAttribute().Class("navigation flex jc-space-between ai-center"),
                        new ALink("/documentation", new Div(
                            new HtmlAttribute().Class("clickable-element"),
                            new Text("Документация")
                        )),
                        new ALink("https://t.me/+X1GHe9cpAQg2Nzdi",
                            new HtmlAttribute().Custom("target=\"_blank\""),
                            new Div(
                                new HtmlAttribute().Class("clickable-element"),
                                new Text("Сообщество")
                            )
                        ),
                        new ALink("https://t.me/vitalySharp",
                            new HtmlAttribute().Custom("target=\"_blank\""),
                            new Div(
                                new HtmlAttribute().Class("clickable-element"),
                                new Text("Связь с разработчиком")
                            )
                        )
                    )
                ),
                new Div(
                    new HtmlAttribute().Class("links flex jc-space-between ai-center"),
                    new ALink("/download", new Div(
                        new HtmlAttribute().Class("clickable-element"),
                        new Div(new Text("Скачать")),
                        new Image("Res/Download.svg", "Download")
                    )),
                    new ALink("https://github.com/Vitaly05/html-creator-library",
                        new HtmlAttribute().Custom("target=\"_blank\""),
                        new Div(
                            new HtmlAttribute().Class("clickable-element"),
                            new Div(new Text("GitHub")),
                            new Image("Res/Open Link.svg", "Open Link")
                        )
                    )
                )
            );

            var mainInnerComponents = new List<BodyComponent>();

            mainInnerComponents.Add(
                new Div(
                    new HtmlAttribute().Class("navigation flex column-direction jc-center ai-start"),
                    new ALink($"/documentation?topic={Topics.Start}",new Div(
                        new HtmlAttribute().Class($"{(model.Title == Topics.Start ? "selected" : "")}"),
                        new Text(Topics.Start)
                    )),
                    new ALink($"/documentation?topic={Topics.Namespaces}", new Div(
                        new HtmlAttribute().Class($"{(model.Title == Topics.Namespaces ? "selected" : "")}"),
                        new Text(Topics.Namespaces)
                    )),
                    new ALink($"/documentation?topic={Topics.StaticClasses}", new Div(
                        new HtmlAttribute().Class($"{(model.Title == Topics.StaticClasses ? "selected" : "")}"),
                        new Text(Topics.StaticClasses)
                    )),
                    new ALink($"/documentation?topic={Topics.Examples}", new Div(
                        new HtmlAttribute().Class($"{(model.Title == Topics.Examples ? "selected" : "")}"),
                        new Text(Topics.Examples)
                    ))
                )
            );

            var articleInnerComponents = new List<BodyComponent>();
            articleInnerComponents.Add(new Text(model.Title, TextType.H1, new HtmlAttribute().Class("article-title")));

            var articleElements = new List<BodyComponent>();
            foreach (var element in model.ArticleElements)
            {
                articleElements.Add(CreateArticleBlock(new ArticleElementModel
                {
                    CanEdit = model.CanEdit,
                    ArticleElement = element
                }));
            }
            articleInnerComponents.AddRange(articleElements);
            if (model.CanEdit)
            {
                articleInnerComponents.Add(
                    new Div(
                        new HtmlAttribute().Class("new-item"),
                        new Button(
                            new Text($"+ Добавить блок в \"{model.Title}\"")
                        )
                    )
                );
            }
            mainInnerComponents.Add(
                new Div(
                    new HtmlAttribute().Class("article").Data("block-type", "article"),
                    articleInnerComponents.ToArray()
                )
            );

            var subNavigationInnerComponents = new List<BodyComponent>();

            if (model.HasTitle)
            {
                var navItems = new List<BodyComponent>();
                foreach (var element in model.GetTitles())
                {
                    navItems.Add(new Div(
                        new Image("Res/List Item.svg", "-"),
                        new ALink($"#{element.Title}", 
                            new HtmlAttribute().Class("clickable-element"),
                            new Div(
                                new Text(element.Title)
                            )
                        )
                    ));
                }
                subNavigationInnerComponents.Add(
                    new Div(
                        new HtmlAttribute().Class("nav"),
                        new Div(
                            new HtmlAttribute().Class("top flex ai-center"),
                            new Image("Res/List.svg", "List"),
                            new Div(
                                new HtmlAttribute().Class("title"),
                                new Text("В этой статье")
                            )
                        ),
                        new Div(
                            new HtmlAttribute().Class("bottom flex column-direction"),
                            navItems.ToArray()
                        )
                    )
                );
            }
            if (model.CanEdit)
            {
                subNavigationInnerComponents.Add(
                    new Button(
                        new HtmlAttribute().Class("clickable-element").Id("save-button"),
                        new Text("Сохранить")
                    )
                );
            }
            else
            {
                subNavigationInnerComponents.Add(
                    new Button(
                        new HtmlAttribute().Class("clickable-element").Id("edit-article-button"),
                        new Image("Res/Edit.svg", "edit")
                    )
                );
            }

            mainInnerComponents.Add(
                new Div(
                    new HtmlAttribute().Class("sub-navigation flex jc-center"),
                    subNavigationInnerComponents.ToArray()
                )
            );

            if (model.CanEdit)
            {
                mainInnerComponents.Add(
                    new Div(
                        new HtmlAttribute().Class("dialog").Id("new-block-dialog").Title("Добавление блока"),
                        new Form(
                            new Select(
                                new HtmlAttribute().Id("block-type-select").Name("type"),
                                new Option("description", true, new Text("Текст")),
                                new Option("code", false, new Text("Код")),
                                new Option("list", false, new Text("Список")),
                                new Option("tip", false, new Text("Совет"))
                            ),
                            new Div(
                                new HtmlAttribute().Id("single-item"),
                                new Label(new HtmlAttribute().For("value")),
                                new TextArea(new HtmlAttribute().Name("value").Custom("type=\"text\""))
                            ),
                            new Div(
                                new HtmlAttribute().Class("list-items"),
                                new Div(
                                    new HtmlAttribute().Class("list-item"),
                                    new Label(
                                        new HtmlAttribute().For("value"),
                                        new Text("Элемент")
                                    ),
                                    new TextArea(new HtmlAttribute().Name("value").Custom("type=\"text\"")),
                                    new Button(
                                        new HtmlAttribute().Class("remove-list-item-button clickable-element").Custom("type=\"button\""),
                                        new Image("Res/Delete.svg", "del")
                                    )
                                ),
                                new Button(
                                    new HtmlAttribute().Class("clickable-element").Id("new-list-item-button").Custom("type=\"button\""),
                                    new Text("+ Добавить")
                                )
                            )
                        )
                    )
                );
            }
            else
            {
                mainInnerComponents.Add(
                    new Div(
                        new HtmlAttribute().Class("dialog").Id("auth-dialog").Title("Авторизация"),
                        new Form(
                            new HtmlAttribute().Custom("autocomplete=\"off\""),
                            new Div(
                                new Label(
                                    new HtmlAttribute().For("login"),
                                    new Text("Логин")
                                ),
                                new Input(InputType.Text, new HtmlAttribute().Name("login").Id("login-field"))
                            ),
                            new Div(
                                new Label(
                                    new HtmlAttribute().For("password"),
                                    new Text("Пароль")
                                ),
                                new Input(InputType.Password, new HtmlAttribute().Name("password").Id("password-field"))
                            )
                        )
                    )
                );
            }

            var main = new Main(
                new HtmlAttribute().Class("main-documentation"),
                mainInnerComponents.ToArray()
            );


            var footer = new Footer(
                new HtmlAttribute().Class("flex jc-center ai-center"),
                new Text("Copyright © 2023 Лозюк Виталий")
            );

            BodyScript script;

            if (model.CanEdit)
            {
                script = new BodyScript("Js/editDocs.js");
            }
            else
            {
                script = new BodyScript("Js/docs.js");
            }

            Body body = new Body();
            body.SetContext(
                header,
                main,
                footer,
                script
            );

            HTML documentation = new HTML(body, head);

            return documentation.GetStringHtml();
        }

        public string GetArticleBlockHtml(ArticleElementModel model)
        {
            return CreateArticleBlock(model).GetStringHtml();
        }
        public BodyComponent CreateArticleBlock(ArticleElementModel model)
        {
            var innerElements = new List<BodyComponent>();
            innerElements.Add(CreateArticleEditTools(new ArticleEditToolsModel { Target = "block", CanEdit = model.CanEdit }));

            if (!String.IsNullOrEmpty(model.ArticleElement.Title))
            {
                innerElements.Add(new Text(model.ArticleElement.Title, TextType.H2,
                    new HtmlAttribute().Class("title block-title text-center").Id(model.ArticleElement.Title)
                ));
            }
            if (model.CanEdit)
                innerElements.Add(new Div(new HtmlAttribute().Class("empty")));

            foreach (var innerElement in model.ArticleElement.InnerElements)
                innerElements.Add(CreateArticleElement(new ArticleElementModel
                    {
                        CanEdit = model.CanEdit,
                        ArticleElement = innerElement
                    }
                ));

            if (model.CanEdit)
            {
                string blockName = model.ArticleElement.Title != String.Empty ? model.ArticleElement.Title : model.ArticleElement.Type;
                innerElements.Add(new Div(
                    new HtmlAttribute().Class("new-item"),
                    new Button(
                        new Text($"+ Добавить блок в \"{blockName}")
                    )
                ));
            }

            var block = new Div(
                new HtmlAttribute().Class($"{model.ArticleElement.Type} block-item").Data("block-type", model.ArticleElement.Type),
                innerElements.ToArray()
            );

            return block;
        }

        public string GetArticleElementHtml(ArticleElementModel model)
        {
            return CreateArticleElement(model).GetStringHtml();
        }
        public BodyComponent CreateArticleElement(ArticleElementModel model)
        {
            BodyComponent articleElement;

            if (model.ArticleElement.Type == ArticleElementsTypes.Description)
            {
                var description = new Div(
                    new HtmlAttribute().Class("text block block-item").Data("block-type", model.ArticleElement.Type),
                    CreateArticleEditTools(new ArticleEditToolsModel { Target = "block", CanEdit = model.CanEdit }),
                    new Div(
                        new HtmlAttribute().Class("value"),
                        new Text(model.ArticleElement.Value)
                    )
                );
                articleElement = description;
            }
            else if (model.ArticleElement.Type == ArticleElementsTypes.List)
            {
                var innerItems = new List<BodyComponent>();
                innerItems.Add(CreateArticleEditTools(new ArticleEditToolsModel { Target = "block", CanEdit = model.CanEdit }));

                foreach (var value in model.ArticleElement.ListValues)
                {
                    innerItems.Add(new Div(
                        new HtmlAttribute().Class("list-item flex"),
                        new Image("Res/List Item.svg", "-"),
                        new Div(
                            new HtmlAttribute().Class("value"),
                            new Text(value)
                        )
                    ));
                }
                var list = new Div(
                    attribute: new HtmlAttribute().Class("list block block-item").Data("block-type", model.ArticleElement.Type),
                    innerItems.ToArray()
                );

                articleElement = list;
            }
            else if (model.ArticleElement.Type == ArticleElementsTypes.Tip)
            {
                articleElement = new Div(
                    new HtmlAttribute().Class("block flex column-direction jc-space-between block-item").Data("block-type", model.ArticleElement.Type),
                    CreateArticleEditTools(new ArticleEditToolsModel { Target = "block", CanEdit = model.CanEdit }),
                    new Div(
                        new HtmlAttribute().Class("tip"),
                        new Div(
                            new HtmlAttribute().Class("flex"),
                            new Image("Res/Tip.svg", "Tip"),
                            new Div(
                                new HtmlAttribute().Class("title"),
                                new Text("Совет")
                            )
                        ),
                        new Div(
                            new HtmlAttribute().Class("value"),
                            new Text(model.ArticleElement.Value)
                        )
                    )
                );
            }
            else if (model.ArticleElement.Type == ArticleElementsTypes.Code)
            {
                var innerItems = new List<BodyComponent>();

                int linesCount = 1;
                for (int iii = 0; iii < model.ArticleElement.ListValues.Count(); ++iii)
                {
                    if (!String.IsNullOrWhiteSpace(model.ArticleElement.ListValues[iii]))
                    {
                        innerItems.Add(new Div(
                            new HtmlAttribute().Class("line"),
                            new Text(linesCount++.ToString())
                        ));
                        innerItems.Add(new Div(
                            new HtmlAttribute().Class("value"),
                            new Text(model.ArticleElement.ListValues[iii])
                        ));
                    }
                }
                var code = new Div(
                    new HtmlAttribute().Class("code"),
                    innerItems.ToArray()
                );

                var block = new Div(
                    new HtmlAttribute().Class("block block-item").Data("block-type", model.ArticleElement.Type),
                    CreateArticleEditTools(new ArticleEditToolsModel { Target = "block", CanEdit = model.CanEdit }),
                    code
                );

                articleElement = block;
            }
            else
            {
                articleElement = CreateArticleBlock(new ArticleElementModel
                {
                    CanEdit = model.CanEdit,
                    ArticleElement = model.ArticleElement
                });
            }

            return articleElement;
        }

        public BodyComponent CreateArticleEditTools(ArticleEditToolsModel model)
        {
            return new Div(
                new HtmlAttribute().Class("tools"),
                new Image("Res/UpDown.svg", "move", new HtmlAttribute()
                    .Class("clickable-element move-button")
                    .Data("target", model.Target)
                ),
                new Image("Res/Delete.svg", "delete", new HtmlAttribute()
                    .Class("clickable-element delete-button")
                )
            );
        }
    }
}
