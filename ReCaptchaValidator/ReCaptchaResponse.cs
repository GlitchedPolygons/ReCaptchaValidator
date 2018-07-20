using System;
using Newtonsoft.Json;

namespace GlitchedPolygons.Services.ReCaptchaValidator
{
    /// <summary>
    /// Google ReCaptchaCode Response JSON
    /// </summary>
    public struct ReCaptchaResponse
    {
        /// <summary>
        /// Bot or not?
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// Timestamp of the challenge load (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        /// </summary>
        [JsonProperty("challenge_ts")]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// The hostname of the site where the reCAPTCHA was solved
        /// </summary>
        [JsonProperty("hostname")]
        public string HostName { get; set; }

        /// <summary>
        /// Any eventual error codes.
        /// </summary>
        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}

// Copyright (C) - Raphael Beck, 2018
