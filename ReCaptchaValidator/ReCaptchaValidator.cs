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

using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace GlitchedPolygons.Services.ReCaptchaValidator
{
    /// <inheritdoc />
    /// <summary>
    /// Validate reCAPTCHA codes from forms with this nifty utility class.
    /// </summary>
    public class ReCaptchaValidator : IReCaptchaValidator
    {
        private const string MEDIA_TYPE = "application/x-www-form-urlencoded";
        private static readonly Uri BASE_URI = new Uri("https://www.google.com", UriKind.Absolute);
        private static readonly Uri VALIDATION_ENDPOINT_URI = new Uri("/recaptcha/api/siteverify", UriKind.Relative);

        private readonly double timeout;
        private readonly string reCaptchaSiteKey;
        private readonly string reCaptchaSecretKey;

        /// <summary>
        /// Gets the reCAPTCHA's public site key.
        /// </summary>
        /// <returns>The public site key.</returns>
        public string GetSiteKey() => reCaptchaSiteKey;

        /// <summary>
        /// Creates a reCAPTCHA validator service instance using the provided API keys
        /// and the specified timeout (in seconds).
        /// </summary>
        /// <param name="reCaptchaSiteKey">The public key.</param>
        /// <param name="reCaptchaSecretKey">The private key.</param>
        /// <param name="timeout">The timeout delay in seconds.</param>
        public ReCaptchaValidator(string reCaptchaSiteKey, string reCaptchaSecretKey, double timeout = 20.0d)
        {
            if (string.IsNullOrEmpty(reCaptchaSiteKey) || string.IsNullOrEmpty(reCaptchaSecretKey))
            {
                throw new ArgumentException($"{nameof(ReCaptchaValidator)}::ctor: The passed {nameof(reCaptchaSiteKey)} or {nameof(reCaptchaSecretKey)} parameter is either null or empty!");
            }

            const double MIN = 5.0d;
            const double MAX = 180.0d;
            double t = Math.Abs(timeout);

            this.timeout = (t < MIN) ? MIN : (t > MAX) ? MAX : t;

            this.reCaptchaSiteKey = reCaptchaSiteKey;
            this.reCaptchaSecretKey = reCaptchaSecretKey;
        }

        /// <summary>
        /// Submits the reCAPTCHA code that you got back from a reCAPTCHA-enabled form
        /// to the Google servers and returns the response (deserialized JSON).
        /// </summary>
        /// <param name="code">The generated reCAPTCHA validation code from the form.</param>
        /// <param name="ip">The request's IP Address.</param>
        /// <returns>Deserialized JSON of Google's response to the submitted reCAPTCHA code.</returns>
        public async Task<ReCaptchaResponse> Validate(string code, IPAddress ip)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException($"{nameof(ReCaptchaValidator)}::{nameof(Validate)}: The passed {nameof(code)} argument is either null or empty! Please don't do this...");
            }

            if (ip is null)
            {
                throw new ArgumentNullException($"{nameof(ReCaptchaValidator)}::{nameof(Validate)}: The passed {nameof(ip)} address parameter is null! Please only pass valid IP addresses to this method... (use \"Request.HttpContext.Connection.RemoteIpAddress\")");
            }

            IPAddress remoteIP = ip.MapToIPv4();
            string payload = $"&secret={reCaptchaSecretKey}&remoteip={remoteIP}&response={code}";

            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(timeout);
                httpClient.BaseAddress = BASE_URI;
                
                using (var httpRequest = new HttpRequestMessage())
                {
                    httpRequest.Method = HttpMethod.Post;
                    httpRequest.RequestUri = VALIDATION_ENDPOINT_URI;
                    httpRequest.Content = new StringContent(payload, Encoding.UTF8, MEDIA_TYPE);
                    
                    HttpResponseMessage response = await httpClient.SendAsync(httpRequest);
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
                }
            }
        }
    }
}
