using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using MvcTest.Models;

namespace MvcTest.Extensions
{
    public static class ModelExtensions
    {
        public static IHtmlContent ColumnOrderHtml<TModel>(this TModel source, string sourceColumn, string columnText, IHttpContextAccessor httpContext) where TModel : IndexViewModel
        {
            if (string.IsNullOrWhiteSpace(sourceColumn))
                return new HtmlString(columnText);

            var requestUrl = httpContext.HttpContext.Request.QueryString.Value;
            var isCurrentColumn = false;
            var isDescendingOrder = string.IsNullOrEmpty(source.OrderDirection.ToString()) || source.OrderDirection.ToString().ToLower() == "desc";

            var link = requestUrl;
            const string linkFormat = "<a href=\"/{0}\" style=\"text-decoration: none;\">{2}&nbsp;<i class=\"{1}\"></i></a>";
            if (requestUrl == "")
            {
                link += "?OrderColumn=" + sourceColumn;
                link += "&OrderDirection=Asc";
            }
            else if (!string.IsNullOrWhiteSpace(httpContext.HttpContext.Request.Query["OrderColumn"].ToString()))
            {
                var orderValue = httpContext.HttpContext.Request.Query["OrderColumn"].ToString();
                var directionValue = httpContext.HttpContext.Request.Query["OrderDirection"].ToString();
                link = link.Replace("OrderColumn=" + orderValue, "OrderColumn=" + sourceColumn);
                isCurrentColumn = sourceColumn == source.OrderColumn;
                link = isCurrentColumn
                    ? link.Replace("OrderDirection=" + directionValue, isDescendingOrder ? "OrderDirection=Asc" : "OrderDirection=Desc")
                    : link.Replace("OrderDirection=" + directionValue, "OrderDirection=Desc");
            }
            else
            {
                link += "&OrderColumn=" + sourceColumn;
                link += "&OrderDirection=Desc";
            }

            if (link.Contains("ExportToExcel=True"))
                link = link.Contains("?ExportToExcel=True") ? link.Replace("ExportToExcel=True&", "") : link.Replace("&ExportToExcel=True", "");
            if (link.Contains("ExportToCsv=True"))
                link = link.Contains("?ExportToCsv=True") ? link.Replace("ExportToCsv=True&", "") : link.Replace("&ExportToCsv=True", "");

            return isCurrentColumn
                ? new HtmlString(string.Format(linkFormat, link, isDescendingOrder ? "fa fa-sort-asc" : "fa fa-sort-desc", columnText))
                : new HtmlString(string.Format(linkFormat, link, "fa fa-sort", columnText));
        }

    }
}
