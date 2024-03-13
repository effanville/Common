using System;
using System.Net.Http;
using System.Threading.Tasks;

using Effanville.Common.Structure.Reporting;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Effanville.Common.Structure.WebAccess
{
    /// <summary>
    /// Provides methods for downloading html from a website.
    /// Hopefully works with websites that require cookies.
    /// </summary>
    public class WebDownloader : IDisposable
    {
        private static HttpClient _client = new HttpClient();

        private static IWebDriver _driver;
        private bool _disposedValue;

        /// <summary>
        /// Determines whether the string is well formed as a url.
        /// </summary>
        public static bool IsValidWebAddress(string address)
        {
            if (!Uri.TryCreate(address, UriKind.Absolute, out Uri uri) || null == uri)
            {
                return false;
            }

            return Uri.IsWellFormedUriString(address, UriKind.Absolute);
        }

        /// <summary>
        /// Downloads from url synchronously.
        /// </summary>
        public static string DownloadFromURL(string url, IReportLogger reportLogger = null)
            => DownloadFromURLasync(url, reportLogger).Result;

        /// <summary>
        /// downloads the data from url asynchronously.
        /// </summary>
        public static async Task<string> DownloadFromURLasync(string url, IReportLogger reportLogger = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                reportLogger?.Error(nameof(WebDownloader), $"Url was empty.");
                return string.Empty;
            }

            string output = string.Empty;
            if (!IsValidWebAddress(url))
            {
                reportLogger?.Error(nameof(WebDownloader), $"Url {url} is not a valid web address.");
                return string.Empty;
            }

            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage
                {
                    RequestUri = new Uri(url),
                    Method = HttpMethod.Get,
                };


                HttpResponseMessage response = await _client.SendAsync(requestMessage).ConfigureAwait(false);
                _ = response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (string.IsNullOrEmpty(result))
                {
                    reportLogger?.Warning(nameof(WebDownloader), $"No data retrieved from url {url}");
                }
                else
                {
                    output = result;
                }
            }
            catch (Exception ex)
            {
                reportLogger?.Log(ReportSeverity.Critical, ReportType.Error, nameof(WebDownloader), $"Failed to download from url {url}. Reason : {ex.Message}");
                return output;
            }

            return output;
        }

        /// <summary>
        /// Returns a cached instance of a web driver.
        /// </summary>
        public static IWebDriver GetCachedInstance(bool forceNew)
        {
            if (_driver == null || forceNew)
            {
                var options = new FirefoxOptions();
                options.AddArguments("--headless");
                _driver = new FirefoxDriver(options);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);
            }

            return _driver;
        }

        /// <summary>
        /// Returns the element text from the specified elememnt from the web driver.
        /// </summary>
        public static string GetElementText(IWebDriver driver, string url, string elementId, int msDelay, IReportLogger logger = null)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                string pageSource = driver.PageSource;
                _ = Task.Delay(msDelay);
                IWebElement element = driver.FindElement(By.Id(elementId));
                _ = Task.Delay(msDelay);
                return element.Text;
            }
            catch (Exception ex)
            {
                logger?.Log(ReportType.Error, "Downloading", ex.Message);
            }

            return null;
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                    _driver.Dispose();
                    _driver.Quit();
                }

                _client = null;
                _driver = null;
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
