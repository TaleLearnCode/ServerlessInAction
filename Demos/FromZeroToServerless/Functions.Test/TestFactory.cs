using Functions.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Functions.Tests
{
  public class TestFactory
  {

    // This property returns an IEnumerable collection of sample data.  The key
    // value paris represen values that are passed into a query string.
    public static IEnumerable<object[]> Data()
    {
      return new List<object[]>
            {
                new object[] { "name", "Bill" },
                new object[] { "name", "Paul" },
                new object[] { "name", "Steve" }

            };
    }

    // This method accepts a key/value pair as arguments and returns a new
    // Dictionary used to create QueryCollection to represent query string values.
    private static Dictionary<string, StringValues> CreateDictionary(string key, string value)
    {
      var qs = new Dictionary<string, StringValues>
      {
        { key, value }
      };
      return qs;
    }

    // This method creates an HTTP request initialized with the given query string parameters.
    public static DefaultHttpRequest CreateHttpRequest(string queryStringKey, string queryStringValue)
    {
      var request = new DefaultHttpRequest(new DefaultHttpContext())
      {
        Query = new QueryCollection(CreateDictionary(queryStringKey, queryStringValue))
      };
      return request;
    }

    // Based on the logger type, this method returns a logger class used for
    // testing.  The ListLogger keeps track of logged messages available for
    // evaluation in tests.
    public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
    {
      ILogger logger;

      if (type == LoggerTypes.List)
      {
        logger = new ListLogger();
      }
      else
      {
        logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
      }

      return logger;
    }

  }

}