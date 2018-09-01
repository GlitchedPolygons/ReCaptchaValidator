using Xunit;
using System;
using System.Net;

namespace GlitchedPolygons.Services.ReCaptchaValidator.UnitTests
{
    public class ValidatorTests
    {
        // The validation's request payload should NEVER contain invalid data.
        // Only the good stuff should be submitted to the Google reCAPTCHA endpoints.

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_SiteKeyArgumentNullOrEmpty_ShouldThrowArgumentException(string siteKey)
        {
            Assert.Throws<ArgumentException>(() => new ReCaptchaValidator(siteKey, "secret_key"));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Ctor_SecretKeyArgumentNullOrEmpty_ShouldThrowArgumentException(string secretKey)
        {
            Assert.Throws<ArgumentException>(() => new ReCaptchaValidator("site_key", secretKey));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async void Validate_CodeArgumentNullOrEmpty_ShouldThrowArgumentException(string code)
        {
            IReCaptchaValidator validator = new ReCaptchaValidator("site_key", "secret_key");
            await Assert.ThrowsAsync<ArgumentException>(() => validator.Validate(code, IPAddress.Any));
        }

        [Fact]
        public async void Validate_IPArgumentNull_ShouldThrowArgumentNullException()
        {
            IReCaptchaValidator validator = new ReCaptchaValidator("site_key", "secret_key");
            await Assert.ThrowsAsync<ArgumentNullException>(() => validator.Validate("code", null));
        }
    }
}
