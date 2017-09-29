using System;
using System.Collections.Generic;

namespace TravelTracker.User
{
    public class DisplayAllViewModel
    {
        public IEnumerable<UserViewModel> Users { get; set; }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
    }
}
