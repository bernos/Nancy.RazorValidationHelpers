using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nancy.ViewEngines.Razor
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ValidationMessagesFor<T>(this HtmlHelpers<T> helpers, string memberName)
        {
            return ValidationMessagesFor(helpers, memberName, memberName);
        }

        public static IHtmlString ValidationMessagesFor<T>(this HtmlHelpers<T> helpers, string memberName, string displayName)
        {
            var msg = helpers.RenderContext.Context.ModelValidationResult.Errors
                        .Where(e => e.MemberNames.Any(m => m == memberName))
                        .Select(e => e.GetMessage(displayName))
                        .ToArray();

            return new NonEncodedHtmlString(String.Join(" ", msg));
        }

        public static IHtmlString ValidationSummary<T>(this HtmlHelpers<T> helpers)
        {
            var sb = new StringBuilder();

            if (!helpers.RenderContext.Context.ModelValidationResult.IsValid)
            {
                sb.AppendLine("<ul>");

                foreach (var error in helpers.RenderContext.Context.ModelValidationResult.Errors)
                {
                    foreach (var memberName in error.MemberNames)
                    {
                        sb.AppendLine(string.Format("<li>{0}</li>", error.GetMessage(memberName)));
                    }
                }

                sb.AppendLine("</ul>");
            }

            return new NonEncodedHtmlString(sb.ToString());
        }

        public static IHtmlString ValidationClassFor<T>(this HtmlHelpers<T> helpers, string memberName)
        {
            if (helpers.RenderContext.Context.ModelValidationResult.Errors.Any(e => e.MemberNames.Contains(memberName)))
            {
                return new NonEncodedHtmlString("has-error");
            }
            return new NonEncodedHtmlString("");
        }
    }
}
