using System.Threading.Tasks;

using Effanville.Common.Structure.WebAccess;

using NUnit.Framework;

namespace Effanville.Common.Structure.Tests.WebAccess
{
    [TestFixture]
    public sealed class WebDownloaderTests
    {
        [TestCase("https://uk.finance.yahoo.com/quote/VWRL.L")]
        [TestCase("https://markets.ft.com/data/funds/tearsheet/summary?s=gb00b4khn986:gbx")]
        public async Task DownloadTest(string url)
        {
            string data = await WebDownloader.DownloadFromURLasync(url, addCookie: false, null).ConfigureAwait(false);
            Assert.That(data, Is.Not.Empty);
        }
    }
}
