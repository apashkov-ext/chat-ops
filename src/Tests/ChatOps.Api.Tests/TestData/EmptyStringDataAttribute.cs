using System.Reflection;
using Xunit.Sdk;

namespace ChatOps.Api.Tests.TestData;

internal sealed class EmptyStringDataAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return [ string.Empty ];
        yield return [ " " ];
    }
}