using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace QAWebsite.TagHelpers
{
    [HtmlTargetElement("q-tag", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class QTagTagHelper : AnchorTagHelper
    {
        [HtmlAttributeName("name")]
        public string QTagName { get; set; }

        public QTagTagHelper(IHtmlGenerator generator) : base(generator) { }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;

            this.Action = "SearchByTag";
            this.Controller = "Question";
            this.RouteValues.Add("tag", QTagName);

            output.Attributes.Add("class", "tag");

            output.Content.SetContent(QTagName);

            base.Process(context, output);
        }
    }
}
