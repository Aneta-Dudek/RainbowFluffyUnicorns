using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Questore.TagHelpers
{
    public class MenuItemTagHelper
    {
        [HtmlTargetElement("menu-item", Attributes = "class")]
        public class PanelHeaderTagHelper : TagHelper
        {
            public int CurrentPage { get; set; }
            public string Class { get; set; }
            public string Content { get; set; }
            public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
            {
                output.TagName = "a";
                output.Attributes.SetAttribute("class", Class);
                output.Content.SetContent(Content);
            }
        }
    }
}
