using Microsoft.AspNetCore.Mvc;

namespace TravelTracker.User
{
    public class UserViewModel
    {
        private IUrlHelper url;

        public UserViewModel(IUrlHelper url)
        {
            this.url = url;
        }

        public string UserName { get; set; }

        public string RouteUrl => url.RouteUrl("users", new { username = UserName, action = "" });
    }
}
