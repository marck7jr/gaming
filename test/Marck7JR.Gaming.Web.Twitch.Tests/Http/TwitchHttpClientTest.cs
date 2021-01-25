using Marck7JR.Core.Extensions;
using Marck7JR.Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Marck7JR.Gaming.Web.Twitch.Http
{
    [TestClass]
    [DeploymentItem("Http/_TwitchHttpClientOptions.json")]
    public class TwitchHttpClientTest
    {
        static TwitchHttpClientTest()
        {
            HostBinder.GetHostBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile($"_{nameof(TwitchHttpClientOptions)}.json");
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddOptions();
                    services.AddHttpClient<TwitchHttpClient>();
                    services.Configure<TwitchHttpClientOptions>(hostContext.Configuration);
                });
        }

        public static TwitchHttpClient? HttpClient { get; set; }

        public TestContext? TestContext { get; set; }

        [ClassInitialize]
        public static void InitializeTestClass(TestContext? _)
        {
            HttpClient = HostBinder.GetHost().Services.GetRequiredService<TwitchHttpClient>();
        }

        [TestMethod]
        public async Task GetOAuthResponse_IsNotNull()
        {
            var oauthResponse = await HttpClient!.GetOAuthResponseAsync();

            Assert.IsNotNull(oauthResponse);

            TestContext?.Write(oauthResponse!.ToJson());
        }
    }
}
