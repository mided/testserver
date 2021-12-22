using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Tests.Integration
{
	public class TestContextMiddleware
	{
		public const string TestFolderHeader = "TestFolder";

        private readonly RequestDelegate _next;

		public TestContextMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, TestContext testContext)
		{
			testContext.TestFolder = context.Request.Headers[TestFolderHeader];

            await _next.Invoke(context);
		}
	}
}
