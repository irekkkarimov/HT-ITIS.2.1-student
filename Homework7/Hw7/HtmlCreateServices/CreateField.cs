using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;

namespace Hw7.HtmlCreateServices;

public class CreateFormHandler
{
    public static IHtmlContent CreateForm(PropertyInfo property, object? model)
    {
        var formDiv = new TagBuilder("div");
        formDiv.InnerHtml.AppendHtml(CreateLabel(property));
        if (property.PropertyType.IsEnum)
            formDiv.InnerHtml.AppendHtml(CreateSelectField(property));
        else
            formDiv.InnerHtml.AppendHtml(CreateInputField(property));
        formDiv.InnerHtml.AppendHtml(CreateValidationField(property, model));

        return formDiv;
    }

    private static IHtmlContent CreateLabel(PropertyInfo property)
    {
        var labelTag = new TagBuilder("label");
        var displayName = property.GetCustomAttributes<DisplayAttribute>()
            .FirstOrDefault()
            ?.Name;

        labelTag.InnerHtml.AppendHtml(displayName ?? SplitCamelCase(property.Name));
        labelTag.Attributes.Add("for", property.Name);
        return labelTag;
    }

    private static string SplitCamelCase(string propertyName)
    {
        var displayName = new StringBuilder("");
        foreach (var letter in propertyName)
        {
            if (letter == propertyName[0])
                displayName.Append(letter);
            else
            {
                if (char.IsUpper(letter))
                    displayName.Append($" {letter}");
                else
                {
                    displayName.Append(letter);
                }
            }
        }
        return displayName.ToString();
    }

    private static IHtmlContent CreateInputField(PropertyInfo property)
    {
        var inputTag = new TagBuilder("input");
        inputTag.Attributes.Add("type", ParseFieldType(property.PropertyType));
        inputTag.Attributes.Add("name", property.Name);
        inputTag.Attributes.Add("id", property.Name);
        return inputTag;
    }

    private static IHtmlContent CreateSelectField(PropertyInfo property)
    {
        var selectTag = new TagBuilder("select");

        foreach (var opt in property.PropertyType.GetEnumNames())
            selectTag.InnerHtml.AppendHtml(CreateOption(opt));
        
        return selectTag;
    }
    
    private static IHtmlContent CreateOption(string optionName)
    {
        var optionTag = new TagBuilder("option");
        optionTag.Attributes.Add("value", optionName);
        optionTag.InnerHtml.AppendHtml(optionName);
        return optionTag;
    }
    
    private static string ParseFieldType(Type propertyType)
    {
        return propertyType.IsValueType ? "number" : "text";
    }

    private static IHtmlContent CreateValidationField(PropertyInfo property, object? model)
    {
        var validationFieldSpan = new TagBuilder("span");
        validationFieldSpan.InnerHtml.AppendHtml(string.Empty);
        if (model == null)
            return validationFieldSpan;

        foreach (ValidationAttribute attribute in property.GetCustomAttributes(typeof(ValidationAttribute), true))
        {
            if (attribute.IsValid(property.GetValue(model)))
                continue;

            validationFieldSpan.InnerHtml.AppendHtml(attribute.ErrorMessage);
            return validationFieldSpan;
        }

        return validationFieldSpan;
    }
}