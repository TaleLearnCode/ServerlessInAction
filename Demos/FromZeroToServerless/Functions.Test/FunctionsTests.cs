using Functions.Test;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Functions.Tests
{
  public class FunctionsTests
  {
    private readonly ILogger logger = TestFactory.CreateLogger();


    // This test creates a request with the query string values of name=Bill to
    // an HTTP function and checks that the expected response is returned.
    [Fact]
    public async void Http_trigger_should_return_known_string()
    {
      var request = TestFactory.CreateHttpRequest("name", "Bill");
      var response = (OkObjectResult)await HttpTrigger.Run(request, logger);
      Assert.Equal("Hello, Bill", response.Value);
    }

    // This test uses xUnit attributes to provide sample data to the HTTP function.
    [Theory]
    [MemberData(nameof(TestFactory.Data), MemberType = typeof(TestFactory))]
    public async void Http_trigger_should_return_known_string_from_member_data(string queryStringKey, string queryStringValue)
    {
      var request = TestFactory.CreateHttpRequest(queryStringKey, queryStringValue);
      var response = (OkObjectResult)await HttpTrigger.Run(request, logger);
      Assert.Equal($"Hello, {queryStringValue}", response.Value);
    }

    // This test creates an instance of ListLogger and passes it to a timer
    // function.  Once the function is run, then the log is checked to ensure
    // the expected message is present.
    [Fact]
    public void Timer_should_log_message()
    {
      var logger = (ListLogger)TestFactory.CreateLogger(LoggerTypes.List);
      TimerTrigger.Run(null, logger);
      var msg = logger.Logs[0];
      Assert.Contains("C# Timer trigger function executed at", msg);
    }
  }
}