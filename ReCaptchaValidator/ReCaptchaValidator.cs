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
            
            this.timeout = Math.Clamp(Math.Abs(timeout), 5.0d, 180.0d);

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

            using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(timeout), BaseAddress = BASE_URI })
            using (var httpRequest = new HttpRequestMessage { Method = HttpMethod.Post, RequestUri = VALIDATION_ENDPOINT_URI, Content = new StringContent(payload, Encoding.UTF8, MEDIA_TYPE) })
            {
                HttpResponseMessage response = await httpClient.SendAsync(httpRequest);
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ReCaptchaResponse>(json);
            }
        }
    }
}

// Copyright (C) - Raphael Beck, 2018
