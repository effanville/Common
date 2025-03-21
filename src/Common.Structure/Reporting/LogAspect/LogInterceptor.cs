using System;
using System.Linq;

using Castle.DynamicProxy;

using Microsoft.Extensions.Logging;

namespace Effanville.Common.Structure.Reporting.LogAspect;

/// <inheritdoc />
[Serializable]
public class LogInterceptor : IInterceptor
{
    /// <inheritdoc />
    public void Intercept(IInvocation invocation)
    {
        ILogInterceptable target = (invocation.InvocationTarget as ILogInterceptable);
        if (target == null)
        {
            invocation.Proceed();
            return;
        }

        object cacheableAttribute = invocation.MethodInvocationTarget
            .GetCustomAttributes(typeof(LogInterceptAttribute), true)
            .FirstOrDefault();
        if (cacheableAttribute == null)
        {
            invocation.Proceed();
            return;
        }

        target.Logger.LogDebug($"{invocation.Method.Name}. Args {string.Join(", ", invocation.Arguments)}");
        try
        {
            invocation.Proceed();
        }
        catch (Exception ex)
        {
            target.Logger.LogError($"{invocation.Method.Name}. Exception={ex}");
            throw;
        }
        finally
        {
            target.Logger.LogDebug($"{invocation.Method.Name}. Return {invocation.ReturnValue}");
        }
    }
}