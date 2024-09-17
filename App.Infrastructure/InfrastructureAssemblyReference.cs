using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public sealed class InfrastructureAssemblyReference
    {
        internal static readonly Assembly assembly = typeof(InfrastructureAssemblyReference).Assembly;
    }
}
