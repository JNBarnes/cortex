using System;
using System.Linq;
using System.Reflection;

namespace Cortex.Util
{
    public class Wpf
    {
        /// <summary>
        /// Fetches WPF assembly.
        /// See http://stackoverflow.com/a/22455498
        /// </summary>
        /// <returns></returns>
        public static Assembly GetAssembly()
        {
            var wpfAssembly = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(item => item.EntryPoint != null)
                .Select(item =>
                    new { item, applicationType = item.GetType(item.GetName().Name + ".App", false) })
                .Where(a => a.applicationType != null && typeof(System.Windows.Application)
                    .IsAssignableFrom(a.applicationType))
                .Select(a => a.item)
            .FirstOrDefault();

            return wpfAssembly;
        }
    }
}
