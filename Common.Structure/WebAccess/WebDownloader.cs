using System;
using System.Net.Http;
using System.Threading.Tasks;

using Common.Structure.Reporting;

namespace Common.Structure.WebAccess
{
    /// <summary>
    /// Provides methods for downloading html from a website.
    /// Hopefully works with websites that require cookies.
    /// </summary>
    public static class WebDownloader
    {
        private static readonly HttpClient client = new HttpClient();

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
        {
            return DownloadFromURLasync(url, reportLogger).Result;
        }

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
                HttpResponseMessage response = await client.SendAsync(requestMessage).ConfigureAwait(false);
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
    }
}
