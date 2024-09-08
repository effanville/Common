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
        /// downloads the data from url asynchronously.
        /// </summary>
        public static async Task<string> DownloadFromURLasync(string url, bool addCookie = false, IReportLogger reportLogger = null)
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
                
                requestMessage.Headers.Add("Connection", "keep-alive");
                requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36 Edg/128.0.0.0");
                requestMessage.Headers.Add("Accept", "*/*");
                if(addCookie)
                {
                    requestMessage.Headers.Add("Cookie", "bbgconsentstring=req1fun1pad1; bdfpc=004.3771631935.1711068963254; _ga=GA1.1.1664482315.1711068963; agent_id=c04fa5f7-6be6-42c2-b35a-7fac373bb047; session_id=f99095c7-e0ab-4444-9601-2f138ceb0a37; _session_id_backup=f99095c7-e0ab-4444-9601-2f138ceb0a37; session_key=302c109daf69e573196ac1165fd2d4aec8734ce5; gatehouse_id=8c312246-8489-49f0-bdaf-990a70045ff6; _pxvid=f50f7e44-e7e6-11ee-bdb4-0b0a525f25ed; consentUUID=1eaa51e4-bac3-445b-8d66-1f135dc97bf2; usnatUUID=6027740b-6915-4459-8057-76ef431ff354; _uetvid=f5cedf50e7e611ee8410771eb52265ec; afUserId=94de923c-9849-4c95-a39e-6757214238c8-p; __gads=ID=f140f78eabf69755:T=1711068965:RT=1711068965:S=ALNI_MYMVOw1H3A27kRfxI01XyKDKqgalQ; __eoi=ID=2250c158e23e961c:T=1711068965:RT=1711068965:S=AA-AfjbYbapOHMuMZVkqU2aFebig; _clck=1bfivdo%7C2%7Cfka%7C0%7C1542; _cc_id=b952f2362570ce66ae68d0d01ef81f12; _scid=de426731-08df-4496-94d0-f59424f8f7b7; _scid_r=de426731-08df-4496-94d0-f59424f8f7b7; _sctr=1%7C1711036800000; __stripe_mid=ec8b4bb4-2c9a-4a50-a57b-239d5eb129e6ceef10; _ga_GQ1PBLXZCT=GS1.1.1711068963.1.1.1711069067.0.0.0; geo_info=%7B%22countryCode%22%3A%22HK%22%2C%22country%22%3A%22HK%22%2C%22field_n%22%3A%22hf%22%2C%22trackingRegion%22%3A%22Asia%22%2C%22cacheExpiredTime%22%3A1726328194821%2C%22region%22%3A%22Asia%22%2C%22fieldN%22%3A%22hf%22%7D%7C1726328194821; geo_info={%22country%22:%22HK%22%2C%22region%22:%22Asia%22%2C%22fieldN%22:%22hf%22}|1726328196789; exp_pref=APAC; country_code=HK; seen_uk=1; _user-data=%7B%22status%22%3A%22anonymous%22%7D; _reg-csrf=s%3AfOQnbguGv1d6i7h7iVtgyZPV.FXDqWI6Ad3WxNFAYIalBqq8e5lCiGyIaen71HYdtmXc; _reg-csrf-token=zAy2o34l-OVOHhOuXLY4A_eKxcTtt6V6e8QM; __stripe_sid=ca041b93-c563-4675-a6e7-9cba87cbdf4b15ca81");
                }


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
        /// Returns the element text from the specified element from the web driver.
        /// </summary>
        public static string GetWebpageSource(IWebDriver driver, string url, int msDelay, IReportLogger logger = null)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                string pageSource = driver.PageSource;
                _ = Task.Delay(msDelay);
                return pageSource;
            }
            catch (Exception ex)
            {
                logger?.Log(ReportType.Error, "Downloading", ex.Message);
            }

            return null;
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
