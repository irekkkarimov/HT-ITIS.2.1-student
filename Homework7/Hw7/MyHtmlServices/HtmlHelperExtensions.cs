using System.Reflection;
using System.Text;
using Hw7.HtmlCreateServices;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var modelType = helper.ViewData.ModelExplorer.ModelType;
        var model = helper.ViewData.Model;
        var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var htmlBuilder = new HtmlContentBuilder();
        htmlBuilder.AppendHtml("<div style=\"display:flex; flex-direction: column; justify-content: space-between; width: 200px;\">");

        foreach (var property in properties)
        {
            var field = CreateFormHandler.CreateForm(property, model);
            htmlBuilder.AppendHtml(field);
        }

        htmlBuilder.AppendHtml("</div>");
        return htmlBuilder;
    }
} 