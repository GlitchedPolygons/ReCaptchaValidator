/*
   Copyright (C) 2019 Raphael Beck

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
using Xunit;
using System;
using System.Net;

namespace GlitchedPolygons.Services.ReCaptchaValidator.Tests
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
