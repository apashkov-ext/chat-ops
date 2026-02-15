using System.Reflection;
using Telegram.Bot.Types;
using Xunit.Sdk;

namespace ChatOps.Api.Tests.TestData;

internal sealed class CorrectMessageDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return
        [
            new Message { Text = "test" }
        ];
    }
}