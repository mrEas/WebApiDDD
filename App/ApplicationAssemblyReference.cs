using System.Reflection;

namespace App.Application
{
    public sealed class ApplicationAssemblyReference
    {
        internal static readonly Assembly assembly = typeof(ApplicationAssemblyReference).Assembly;
    }
}
