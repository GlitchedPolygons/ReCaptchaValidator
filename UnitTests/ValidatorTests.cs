using Xunit;
using System;
using System.Net;

namespace GlitchedPolygons.Services.ReCaptchaValidator.UnitTests
{
    public class ValidatorTests
    {
        // The validation's request payload should NEVER contain invalid data.
        // Only the good stuff should be submitted to the Google reCAPTCHA endpoints.

        [Fact]
        public void Ctor_SiteKeyArgumentNull_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ReCaptchaValidator(null, "secret_key"));
        }

        [Fact]
        public void Ctor_SecretKeyArgumentNull_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ReCaptchaValidator("site_key", null));
        }

        [Fact]
        public void Ctor_SiteKeyArgumentEmpty_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ReCaptchaValidator(string.Empty, "secret_key"));
        }

        [Fact]
        public void Ctor_SecretKeyArgumentEmpty_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ReCaptchaValidator("site_key", string.Empty));
        }

        [Fact]
        public async void Validate_CodeArgumentNull_ShouldThrowArgumentException()
        {
            IReCaptchaValidator validator = new ReCaptchaValidator("site_key", "secret_key");
            await Assert.ThrowsAsync<ArgumentException>(() => validator.Validate(null, IPAddress.Any));
        }

        [Fact]
        public async void Validate_CodeArgumentEmpty_ShouldThrowArgumentException()
        {
            IReCaptchaValidator validator = new ReCaptchaValidator("site_key", "secret_key");
            await Assert.ThrowsAsync<ArgumentException>(() => validator.Validate(string.Empty, IPAddress.Any));
        }

        [Fact]
        public async void Validate_IPArgumentNull_ShouldThrowArgumentNullException()
        {
            IReCaptchaValidator validator = new ReCaptchaValidator("site_key", "secret_key");
            await Assert.ThrowsAsync<ArgumentNullException>(() => validator.Validate("code", null));
        }
    }
}
