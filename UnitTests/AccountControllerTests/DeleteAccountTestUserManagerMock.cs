using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UnitTests.Helpers;

namespace UnitTests.AccountControllerTests
{
    public class DeleteAccountTestUserManagerMock : UserManagerMock
    {
        public new List<IdentityUser> Users { get; set; }

        public override Task<IdentityUser> FindByNameAsync(string userName)
        {
            return Task.FromResult(Users.Single(x => x.UserName.Equals(userName)));
        }

        public override Task<IdentityResult> DeleteAsync(IdentityUser user)
        {
            Users.Remove(user);

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
