using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Moq;
using System;
using TravelTracker.User;
using Microsoft.AspNetCore.Builder;

namespace UnitTests
{
    public class IdentityOptionsProviderTests
    {
        [Fact]
        public void EmptyConfigThrowsExceptionTest()
        {
            GivenEmptyConfig();

            WhenCreateIndentityOptionsProviderThenExecption();
        }

        [Fact]
        public void GivenConfigRightPropertiesTest()
        {
            GivenConfig();

            WhenCreateIdentityOptionsProvider();

            ThenRightProperties();
        }

        [Fact]
        public void GivenConfigRightSetOptionsTest()
        {
            GivenConfig();

            WhenCreateIdentityOptionsProvider();

            ThenRightSetOptions();
        }

        IConfigurationRoot config;
        IdentityOptionsProvider identityOptionsProvider;

        void GivenEmptyConfig()
        {
            var configMock = new Mock<IConfigurationRoot>();
            configMock.Setup(x => x[It.IsAny<string>()]).Returns((string)null);

            config = configMock.Object;
        }

        void WhenCreateIndentityOptionsProviderThenExecption() => Assert.Throws<ArgumentException>(() => new IdentityOptionsProvider(config));

        void GivenConfig()
        {
            config = new TestConfig();
        }

        void WhenCreateIdentityOptionsProvider()
        {
            identityOptionsProvider = new IdentityOptionsProvider(config);
        }

        void ThenRightProperties()
        {
            Assert.True(identityOptionsProvider.PasswordRequireDigit);
            Assert.Equal(6, identityOptionsProvider.PasswordRequiredLength);
            Assert.True(identityOptionsProvider.PasswordRequireNonAlphanumeric);
            Assert.True(identityOptionsProvider.PasswordRequireUppercase);
            Assert.False(identityOptionsProvider.PasswordRequireLowercase);
            Assert.True(identityOptionsProvider.RequireDigitUniqueEmail);
        }

        void ThenRightSetOptions()
        {
            var options = new IdentityOptions();
            identityOptionsProvider.SetOptions(options);

            Assert.True(options.Password.RequireDigit);
            Assert.Equal(6, options.Password.RequiredLength);
            Assert.True(options.Password.RequireNonAlphanumeric);
            Assert.True(options.Password.RequireUppercase);
            Assert.False(options.Password.RequireLowercase);
            Assert.True(options.User.RequireUniqueEmail);
        }
    }

    class TestConfig : IConfigurationRoot
    {
        public TestConfig()
        {
            Values = new Dictionary<string, string>();

            Values.Add("Identity:Password:RequireDigit", "true");
            Values.Add("Identity:Password:RequiredLength", "6");
            Values.Add("Identity:Password:RequireNonAlphanumeric", "true");
            Values.Add("Identity:Password:RequireUppercase", "true");
            Values.Add("Identity:Password:RequireLowercase", "false");
            Values.Add("Identity:RequireUniqueEmail", "true");
        }

        public Dictionary<string, string> Values { get; }

        public string this[string key] 
        { 
            get 
            {
                return Values[key];
            }
            set => throw new NotSupportedException(); 
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotSupportedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotSupportedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotSupportedException();
        }

        public void Reload()
        {
            throw new NotSupportedException();
        }
    }
}
