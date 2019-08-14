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

using System.Net;
using System.Threading.Tasks;

namespace GlitchedPolygons.Services.ReCaptchaValidator
{
    /// <summary>
    /// Interface for validating whether a user is human or not.
    /// </summary>
    public interface IReCaptchaValidator
    {
        /// <summary>
        /// Gets the reCAPTCHA's public site key.
        /// </summary>
        /// <returns>The public site key.</returns>
        string GetSiteKey();

        /// <summary>
        /// Submits the reCAPTCHA code that you got back from a reCAPTCHA-enabled form
        /// to the Google servers and returns the response (deserialized JSON).
        /// </summary>
        /// <param name="code">The generated reCAPTCHA validation code from the form.</param>
        /// <param name="ip">The request's IP Address.</param>
        /// <returns>Deserialized JSON of Google's response to the submitted reCAPTCHA code.</returns>
        Task<ReCaptchaResponse> Validate(string code, IPAddress ip);
    }
}
