using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BddWithSpecFlow.GeekPizza.Web.Utils
{
    public static class ViewHelperExtensions
    {
        public static string Page(this UrlHelper urlHelper, string folderName, string viewName = "Index")
        {
            return urlHelper.Action("ViewPage", "Page", new {folderName, viewName});
        }

        public static IHtmlContent PageLink(this IHtmlHelper urlHelper, string label, string pageName, string cssClass = null)
        {
            var htmlContent = new TagBuilder("a");
            if (cssClass != null)
                htmlContent.AddCssClass(cssClass);
            htmlContent.InnerHtml.Append(label);
            htmlContent.MergeAttribute("href", $"/{pageName}");
            return htmlContent;
            //urlHelper.ActionLink()
            //return new HtmlString($"<a href='/{pageName}'>{label}</a>");
            //return urlHelper.ActionLink(label, "ViewPage", "Page", "http", "localhost:64397", null, new {folderName, viewName}, htmlAttributes);
        }

        public static string BasePath(this UrlHelper urlHelper)
        {
            var homePagePath = urlHelper.Page("Home");
            if (string.IsNullOrEmpty(homePagePath))
                return "/";
            if (!homePagePath.EndsWith("/"))
                homePagePath += "/";
            return homePagePath;
        }

        public static string HomeUrl(this IHtmlHelper htmlHelper)
        {
            return "/";
        }

        public static string Api(this IUrlHelper urlHelper, string path)
        {
            return path;
            //return ((UrlHelper)urlHelper).BasePath() + path.TrimStart('/');
        }
    }
}