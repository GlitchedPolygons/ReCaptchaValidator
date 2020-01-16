[![License Shield](https://img.shields.io/badge/license-Apache--2.0-orange)](https://github.com/GlitchedPolygons/Cryptography.Asymmetric/blob/master/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/GlitchedPolygons.Services.ReCaptchaValidator.svg)](https://www.nuget.org/packages/GlitchedPolygons.Services.ReCaptchaValidator) [![CircleCI Status Badge](https://circleci.com/gh/GlitchedPolygons/ReCaptchaValidator.svg?style=shield)](https://circleci.com/gh/GlitchedPolygons/ReCaptchaValidator) [![Build Status](https://travis-ci.org/GlitchedPolygons/ReCaptchaValidator.svg?branch=master)](https://travis-ci.org/GlitchedPolygons/ReCaptchaValidator)

# ReCaptcha Validator Service

Google ReCaptcha wrapper class (injectable service for ASP.NET Core apps).
Make sure to initialize the class with valid API Keys!

Inject into ASP.NET Core apps in Startup.cs by calling services.AddTransient

## Dependencies

* Newtonsoft.Json v11.0.2
* xunit NuGet packages
