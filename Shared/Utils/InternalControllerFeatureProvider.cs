using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Shared.Utils;

public class InternalControllerFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        if (!typeInfo.IsClass) return false;
        if (typeInfo.IsAbstract) return false;
        if (typeInfo.ContainsGenericParameters) return false;
        if (typeInfo.IsDefined(typeof(NonControllerAttribute))) return false;

        // We intentionally omit the `!typeInfo.IsPublic` check that the base class has.
        return typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ||
               typeInfo.IsDefined(typeof(ControllerAttribute));
    }
}