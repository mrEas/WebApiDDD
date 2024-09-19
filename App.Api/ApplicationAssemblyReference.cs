using System.Reflection;

namespace App.Api
{
    public sealed class ApiAssemblyReference
    {
        internal static readonly Assembly assembly = typeof(ApiAssemblyReference).Assembly;
    }
}
