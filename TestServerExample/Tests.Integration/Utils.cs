using System.IO;
using System.Threading.Tasks;
using FluentAssertions.Json;
//using FluentAssertions;
using Newtonsoft.Json.Linq;

namespace Tests.Integration
{
    public static class Utils
    {
        public static async Task CompareStringWithFile(string content, string fileName)
        {
            var shouldBeContent = await File.ReadAllTextAsync(fileName);

            var expected = JToken.Parse(shouldBeContent);
            var actual = JToken.Parse(content);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
