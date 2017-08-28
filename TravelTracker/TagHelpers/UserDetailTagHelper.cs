using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TravelTracker.Views.User
{
    public class UserDetailTagHelper : TagHelper
    {
		[HtmlAttributeName("user-detail-name")]
		public string Name { get; set; }

		[HtmlAttributeName("user-detail-value")]
		public string Value { get; set; }

        [HtmlAttributeName("user-detail-content-href")]
        public string Href { get; set; }

        public UserDetailTagHelper()
        {
            var whatever = 0;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var content = GetContent();
            output.Content.SetHtmlContent(content);
        }

        private string GetContent()
        {
			return $@"<div class='panel-heading'> 
                        <dl class='pull-left dl-horizontal no-bottom-margin'>
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
                        <div class='clearfix'></div>
                    </div>";
        }
    }
}
