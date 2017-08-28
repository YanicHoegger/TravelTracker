using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TravelTracker.TagHelpers
{
	public class UserdetailsTagHelper : TagHelper
	{
		[HtmlAttributeName("name")]
		public string Name { get; set; }

		[HtmlAttributeName("value")]
		public string Value { get; set; }

		[HtmlAttributeName("content-href")]
		public string Href { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{		
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "panel-heading");
			output.Content.SetHtmlContent(GetContent());
		}

		private string GetContent()
		{
			return $@"<dl class='pull-left dl-horizontal no-bottom-margin'>
                        <dt>
                            <h4 class='panel-title'>
                                {Name}
                            </h4>
                        </dt>
                        <dd class='visibility-toggled'>
                            {Value}
                        </dd>
                    </dl>
                    <a data-toggle='collapse' data-parent='#accordion' href='{Href}' class='pull-right visibility-toggled'>
                        Edit
                    </a>
                    <div class='clearfix'></div>";
		}
	}
}
