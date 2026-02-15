using System.Reflection;
using Xunit.Sdk;

namespace ChatOps.Api.Tests.TestData;

internal sealed class TelegramMentionDataAttribute : DataAttribute
{
    public override IEnumerable<object?[]> GetData(MethodInfo testMethod)
    {
        yield return [ "Имя", string.Empty, string.Empty, "Имя" ];
        yield return [ "Имя", null, null, "Имя" ];
        
        yield return [ "Имя", "Фамилия", string.Empty, "Имя Фамилия" ];
        yield return [ "Имя", "Фамилия", null, "Имя Фамилия" ];
        
        yield return [ "Имя", string.Empty, "username", "@username" ];
        yield return [ "Имя", null, "username", "@username" ];
        yield return [ "Имя", "Фамилия", "username", "@username" ];
    }
}