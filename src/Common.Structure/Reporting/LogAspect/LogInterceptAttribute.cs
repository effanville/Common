using System;

namespace Effanville.Common.Structure.Reporting.LogAspect;

/// <summary>
/// Attribute to detail that a method can be intercepted for logging
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class LogInterceptAttribute : Attribute
{
}