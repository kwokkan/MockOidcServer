using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Html;

namespace Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelperExtensions
{
    public static IHtmlContent DropDownListForBindProperty<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression, IEnumerable<SelectListItem> selectList)
    {
        var memberExpression = expression.Body as MemberExpression;

        if (memberExpression == null)
        {
            return HtmlString.Empty;
        }

        var bindProperty = memberExpression.Member.GetCustomAttribute<BindPropertyAttribute>(true);

        if (bindProperty == null)
        {
            return HtmlString.Empty;
        }

        var tagBuilder = new TagBuilder("select");

        var id = bindProperty.Name ?? memberExpression.Member.Name;

        tagBuilder.Attributes.Add("id", id);
        tagBuilder.Attributes.Add("name", id);

        if (selectList != null)
        {
            foreach (var selectItem in selectList)
            {
                var optionBuilder = new TagBuilder("option");

                optionBuilder.InnerHtml.Append(selectItem.Text);

                optionBuilder.Attributes.Add("value", selectItem.Value);

                if (selectItem.Disabled)
                {
                    optionBuilder.Attributes.Add("disabled", null);
                }

                if (selectItem.Selected)
                {
                    optionBuilder.Attributes.Add("selected", null);
                }

                tagBuilder.InnerHtml.AppendHtml(optionBuilder);
            }
        }

        return tagBuilder;
    }

    public static IHtmlContent LabelForBindProperty<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
    {
        var memberExpression = expression.Body as MemberExpression;

        if (memberExpression == null)
        {
            return HtmlString.Empty;
        }

        var bindProperty = memberExpression.Member.GetCustomAttribute<BindPropertyAttribute>(true);

        if (bindProperty == null)
        {
            return HtmlString.Empty;
        }

        return htmlHelper.LabelFor(expression, new { @for = bindProperty.Name ?? memberExpression.Member.Name });
    }

    public static IHtmlContent? TextBoxForBindProperty<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
    {
        var memberExpression = expression.Body as MemberExpression;

        if (memberExpression == null)
        {
            return HtmlString.Empty;
        }

        var bindProperty = memberExpression.Member.GetCustomAttribute<BindPropertyAttribute>(true);

        if (bindProperty == null)
        {
            return HtmlString.Empty;
        }

        var tagBuilder = new TagBuilder("input");
        tagBuilder.TagRenderMode = TagRenderMode.SelfClosing;

        var id = bindProperty.Name ?? memberExpression.Member.Name;
        var value = htmlHelper.ValueFor(expression);

        tagBuilder.Attributes.Add("id", id);
        tagBuilder.Attributes.Add("name", id);
        tagBuilder.Attributes.Add("value", value);
        tagBuilder.Attributes.Add("type", "text");

        return tagBuilder;
    }
}
