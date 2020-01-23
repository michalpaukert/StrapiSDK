using System;
using System.Threading.Tasks;
using Xunit;

namespace Strapi_SDK.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task TestMethod1()
        {
            var client = new StrapiClient("http://localhost:1337");

            //await client.Login("deliver", "test123");
            var a = await client.GetEntries<dynamic>("about-uses");

        }
    }
}
